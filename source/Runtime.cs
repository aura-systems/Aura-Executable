using Internal.Runtime;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#region A couple very basic things
namespace System
{
    public sealed class FlagsAttribute : Attribute { }

    public sealed class AttributeUsageAttribute : Attribute
    {
        public AttributeTargets ValidOn { get; set; }
        public bool AllowMultiple { get; set; }
        public bool Inherited { get; set; }

        public AttributeUsageAttribute(AttributeTargets validOn)
        {
            ValidOn = validOn;
            Inherited = true;
        }

        public AttributeUsageAttribute(AttributeTargets validOn, bool allowMultiple, bool inherited)
        {
            ValidOn = validOn;
            AllowMultiple = allowMultiple;
            Inherited = inherited;
        }
    }

    public enum AttributeTargets
    {
        Assembly = 0x0001,
        Module = 0x0002,
        Class = 0x0004,
        Struct = 0x0008,
        Enum = 0x0010,
        Constructor = 0x0020,
        Method = 0x0040,
        Property = 0x0080,
        Field = 0x0100,
        Event = 0x0200,
        Interface = 0x0400,
        Parameter = 0x0800,
        Delegate = 0x1000,
        ReturnValue = 0x2000,
        GenericParameter = 0x4000,

        All = Assembly | Module | Class | Struct | Enum | Constructor |
                        Method | Property | Field | Event | Interface | Parameter |
                        Delegate | ReturnValue | GenericParameter
    }

    public unsafe class Object
    {
        // The layout of object is a contract with the compiler.
        internal unsafe EEType* m_pEEType;

        [StructLayout(LayoutKind.Sequential)]
        private class RawData
        {
            public byte Data;
        }

        internal ref byte GetRawData()
        {
            return ref Unsafe.As<RawData>(this).Data;
        }

        internal uint GetRawDataSize()
        {
            return m_pEEType->BaseSize - (uint)sizeof(ObjHeader) - (uint)sizeof(EEType*);
        }

        public Object() { }
        ~Object() { }


        public virtual bool Equals(object o)
            => false;

        public virtual int GetHashCode()
            => 0;

        public virtual string ToString()
            => "{object}";


        public virtual void Dispose()
        {
            var obj = this;
            //Allocator.Free(Unsafe.As<object, IntPtr>(ref obj)); CALL FREE
        }
    }
    public struct Void { }

    // The layout of primitive types is special cased because it would be recursive.
    // These really don't need any fields to work.
    public struct Boolean { }
    public struct Char { }
    public struct SByte { }
    public struct Byte { }
    public struct Int16 { }
    public struct UInt16 { }
    public struct Int32 { }
    public struct UInt32 { }
    public struct Int64 { }
    public struct UInt64 { }
    public unsafe struct IntPtr
    {
        void* _value;

        public IntPtr(void* value) { _value = value; }
        public IntPtr(int value) { _value = (void*)value; }
        public IntPtr(uint value) { _value = (void*)value; }
        public IntPtr(long value) { _value = (void*)value; }
        public IntPtr(ulong value) { _value = (void*)value; }

        [Intrinsic]
        public static readonly IntPtr Zero;

        //public override bool Equals(object o)
        //	=> _value == ((IntPtr)o)._value;

        public bool Equals(IntPtr ptr)
            => _value == ptr._value;

        //public override int GetHashCode()
        //	=> (int)_value;

        public static explicit operator IntPtr(int value) => new IntPtr(value);
        public static explicit operator IntPtr(uint value) => new IntPtr(value);
        public static explicit operator IntPtr(long value) => new IntPtr(value);
        public static explicit operator IntPtr(ulong value) => new IntPtr(value);
        public static explicit operator IntPtr(void* value) => new IntPtr(value);
        public static explicit operator void*(IntPtr value) => value._value;

        public static explicit operator int(IntPtr value)
        {
            var l = (long)value._value;

            return checked((int)l);
        }

        public static explicit operator long(IntPtr value) => (long)value._value;
        public static explicit operator ulong(IntPtr value) => (ulong)value._value;

        public static IntPtr operator +(IntPtr a, uint b)
            => new IntPtr((byte*)a._value + b);

        public static IntPtr operator +(IntPtr a, ulong b)
            => new IntPtr((byte*)a._value + b);

        public static bool operator ==(IntPtr a, IntPtr b)
        {
            return a._value == b._value;
        }

        public static bool operator !=(IntPtr a, IntPtr b)
        {
            return !(a._value == b._value);
        }
    }
    public struct UIntPtr { }
    public struct Single { }
    public struct Double { }

    public abstract class ValueType { }
    public abstract class Enum : ValueType { }

    public struct Nullable<T> where T : struct { }

    public sealed unsafe class String
    {
        [Intrinsic]
        public static readonly string Empty = "";


        // The layout of the string type is a contract with the compiler.
        int _length;
        internal char _firstChar;


        public int Length
        {
            [Intrinsic]
            get { return _length; }
            internal set { _length = value; }
        }

        public unsafe char this[int index]
        {
            [Intrinsic]
            get
            {
                return Unsafe.Add(ref _firstChar, index);
            }

            set
            {
                fixed (char* p = &_firstChar) p[index] = value;
            }
        }
    }

    public abstract class Array { }
    public abstract class Delegate { }
    public abstract class MulticastDelegate : Delegate { }

    public struct RuntimeTypeHandle { }
    public struct RuntimeMethodHandle { }
    public struct RuntimeFieldHandle { }

    public class Attribute { }

    namespace Runtime.CompilerServices
    {
        public class RuntimeHelpers
        {
            public static unsafe int OffsetToStringData => sizeof(IntPtr) + sizeof(int);
        }

        public static class RuntimeFeature
        {
            public const string UnmanagedSignatureCallingConvention = nameof(UnmanagedSignatureCallingConvention);
        }

        internal sealed class IntrinsicAttribute : Attribute { }

        public static unsafe class Unsafe
        {
            [Intrinsic]
            public static extern ref T Add<T>(ref T source, int elementOffset);

            [Intrinsic]
            public static extern ref TTo As<TFrom, TTo>(ref TFrom source);

            [Intrinsic]
            public static extern T As<T>(object value) where T : class;

            [Intrinsic]
            public static extern void* AsPointer<T>(ref T value);

            [Intrinsic]
            public static extern ref T AsRef<T>(void* pointer);

            public static ref T AsRef<T>(IntPtr pointer)
                => ref AsRef<T>((void*)pointer);

            [Intrinsic]
            public static extern int SizeOf<T>();

            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
            {
                for (; ; );
            }

            [Intrinsic]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static ref T AddByteOffset<T>(ref T source, nuint byteOffset)
            {
                return ref AddByteOffset(ref source, (IntPtr)(void*)byteOffset);
            }
        }

        public sealed class MethodImplAttribute : Attribute
        {
            public MethodImplAttribute(MethodImplOptions methodImplOptions) { }
        }

        public enum MethodImplOptions
        {
            Unmanaged = 0x0004,
            NoInlining = 0x0008,
            NoOptimization = 0x0040,
            AggressiveInlining = 0x0100,
            AggressiveOptimization = 0x200,
            InternalCall = 0x1000,
        }
    }
}

namespace System.Runtime.InteropServices
{
    public class UnmanagedType { }

    sealed class StructLayoutAttribute : Attribute
    {
        public StructLayoutAttribute(LayoutKind layoutKind)
        {
        }
    }

    public sealed class DllImportAttribute : Attribute
    {
        public string EntryPoint;
        public CharSet CharSet;
        public bool SetLastError;
        public bool ExactSpelling;
        public CallingConvention CallingConvention;
        public bool BestFitMapping;
        public bool PreserveSig;
        public bool ThrowOnUnmappableChar;

        public string Value { get; }

        public DllImportAttribute(string dllName)
        {
            Value = dllName;
        }
    }

    internal enum LayoutKind
    {
        Sequential = 0, // 0x00000008,
        Explicit = 2, // 0x00000010,
        Auto = 3, // 0x00000000,
    }

    public enum CharSet
    {
        None = 1,       // User didn't specify how to marshal strings.
        Ansi = 2,       // Strings should be marshalled as ANSI 1 byte chars.
        Unicode = 3,    // Strings should be marshalled as Unicode 2 byte chars.
        Auto = 4,       // Marshal Strings in the right way for the target system.
    }

    public enum CallingConvention
    {
        Winapi = 1,
        Cdecl = 2,
        StdCall = 3,
        ThisCall = 4,
        FastCall = 5,
    }

    public sealed class FieldOffsetAttribute : Attribute
    {
        public FieldOffsetAttribute(int offset)
        {
            Value = offset;
        }

        public int Value { get; }
    }
}
#endregion

#region Things needed by ILC
namespace System
{
    namespace Runtime
    {
        internal sealed class RuntimeExportAttribute : Attribute
        {
            public RuntimeExportAttribute(string entry) { }
        }
    }

    class Array<T> : Array { }
}

namespace Internal.TypeSystem
{
    public enum ExceptionStringID { }
}

namespace Internal.Runtime.CompilerHelpers
{
    using Internal.TypeSystem;
    using System.Runtime;

    // A class that the compiler looks for that has helpers to initialize the
    // process. The compiler can gracefully handle the helpers not being present,
    // but the class itself being absent is unhandled. Let's add an empty class.
    class StartupCodeHelpers
    {
        // A couple symbols the generated code will need we park them in this class
        // for no particular reason. These aid in transitioning to/from managed code.
        // Since we don't have a GC, the transition is a no-op.
        [RuntimeExport("__fail_fast")]
        static void FailFast() { while (true) ; }

        [RuntimeExport("RhpReversePInvoke")]
        static void RhpReversePInvoke(IntPtr frame) { }
        [RuntimeExport("RhpReversePInvokeReturn")]
        static void RhpReversePInvokeReturn(IntPtr frame) { }
        [RuntimeExport("RhpReversePInvoke2")]
        static void RhpReversePInvoke2(System.IntPtr frame) { }
        [RuntimeExport("RhpReversePInvokeReturn2")]
        static void RhpReversePInvokeReturn2(System.IntPtr frame) { }
        [RuntimeExport("RhpPInvoke")]
        static void RhpPInvoke(IntPtr frame) { }
        [RuntimeExport("RhpPInvokeReturn")]
        static void RhpPInvokeReturn(IntPtr frame) { }

        [RuntimeExport("RhpFallbackFailFast")]
        static void RhpFallbackFailFast() { while (true) ; }

        [RuntimeExport("RhpAssignRef")]
        static unsafe void RhpAssignRef(void** address, void* obj)
        {
            *address = obj;
        }

        [RuntimeExport("RhpByRefAssignRef")]
        static unsafe void RhpByRefAssignRef(void** address, void* obj)
        {
            *address = obj;
        }

        [RuntimeExport("RhpCheckedAssignRefEAX")]
        static unsafe void RhpCheckedAssignRefEAX(void** address, void* obj)
        {
            *address = obj;
        }

        [RuntimeExport("RhpCheckedAssignRef")]
        static unsafe void RhpCheckedAssignRef(void** address, void* obj)
        {
            *address = obj;
        }

        [RuntimeExport("memset")]
        static unsafe void MemSet(byte* ptr, int c, int count)
        {
            for (byte* p = ptr; p < ptr + count; p++)
                *p = (byte)c;
        }

        [RuntimeExport("memcpy")]
        static unsafe void MemCpy(byte* dest, byte* src, ulong count)
        {
            for (ulong i = 0; i < count; i++) dest[i] = src[i];
        }
    }

    public static class ThrowHelpers
    {
        public static void ThrowInvalidProgramException(ExceptionStringID id) { }
        public static void ThrowInvalidProgramExceptionWithArgument(ExceptionStringID id, string methodName) { }
        public static void ThrowOverflowException() { }
        public static void ThrowIndexOutOfRangeException() { }
        public static void ThrowTypeLoadException(ExceptionStringID id, string className, string typeName) { }
    }
}
#endregion

#region NET Runtime

public unsafe class Console
{
    [DllImport("*")]
    static extern int printf(char* text);

    [DllImport("*")]
    static extern void clear();

    public static void Clear()
    {
        clear();
    }

    public static void WriteLine(string str)
    {
        fixed (char *unsafeStr = str)
        {
            printf(unsafeStr);
        }
    }
}

#endregion