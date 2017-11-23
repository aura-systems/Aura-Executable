asm (".code32\n"
     "call _auramain\n"
	 "ret\n");
	 
	#ifdef	_BSD_SIZE_T_
	typedef	_BSD_SIZE_T_	size_t;
	#undef	_BSD_SIZE_T_
	#endif

	static void printf(char *string)
    {
       asm volatile ("mov   $0x01, %%eax\n" //Print function
	                 "mov   %0, %%esi\n"
                     "int   $0x48\n"
                     :
                     : "r"(string)
					 : "esi" , "eax", "memory");
    }
	
	static char *fgets(char *string)
    {
		asm volatile ("mov   $0x04, %%eax\n" //Print function
	                 "mov   %0, %%esi\n"
                     "int   $0x48\n"
                     :
                     : "r"(string)
					 : "esi" , "eax", "memory");
				
		char *returned;
		
		asm volatile ("mov   %%edi, %0\n"
                     :
                     : "r"(returned)
					 : "edi" , "eax", "memory");
					 	
		return returned;
    }
	
	static void clear()
    {
       asm volatile ("mov   $0x02, %eax\n" //clear function
                     "int   $0x48\n");		   
    }
	
	static void setcursor(int x, int y)
    {
       asm volatile ("mov   $0x03, %%eax\n" //setcursor_x function
	                 "mov   %0, %%esi\n"
					 :
                     : "r"(x)
					 : "esi", "eax", "memory");

       asm volatile ("mov   %0, %%edi\n"
					 :
					 : "r"(y)
					 : "edi", "memory");

       asm volatile ("int   $0x48\n");							 
    }

	static void resetcursor()
    {
       asm volatile ("mov   $0x31, %eax\n" //resetcursor function
                     "int   $0x48\n");
    }
	
	void *memcpy(void *, const void *, size_t);
	
	/** This is designed to be small, not fast. */
	
	void *memcpy(void *s1, const void *s2, size_t n)
	{
		const char *f = s2;
		char *t = s1;
		
		if (f < t) {
			f += n;
			t += n;
			while (n-- > 0)
				*--t = *--f;
		} else 
			while (n-- > 0)
				*t++ = *f++;
			
		return s1;
	}
	
	void *memset(void *dst, int c, size_t n)
	{
		if (n != 0) {
			char *d = dst;
			
			do
				*dst++ = c;
			while (--n != 0);
		}
		return (dst);
	}
	
	/*
	 * Compare memory regions.
	 */
	int memcmp(const void *s1, const void *s2, size_t n)
	{
		if (n != 0) {
			const unsigned char *p1 = s1, *p2 = s2;
			
			do {
				if (*p1++ != *p2++)
					return (*--p1 - *--p2);
			} while (--n != 0);
		}
		return (0);
	}
	
	void *memchr(const void *s, int c, size_t n)
	{
		if (n != 0) {
			const unsigned char *p = s;
			
			do {
				if (*p++ == c)
					return ((void *)(p - 1));
			} while (--n != 0);
		}
		return ((char *)0);
	}
	
	
	void bcopy(const char *src, char *dst, size_t bytes)
	{
		if (dst >= src && dst < src + bytes) {
			/* do overlapping copy backwards, slowly! */
			src += bytes;
			dst += bytes;
			while (bytes--)
				*--dst = *--src;
		} else {
			/* use possibly assembler code memcpy() */
			memcpy (dst, src, bytes);
		}
	}
	
	int bcmp(const void *b1, const void *b2, size_t length)
	{
		char *p1, *p2;
		
		if (length == 0)
			return(0);
		p1 = (char *)b1;
		p2 = (char *)b2;
		do
			if (*p1++ != *p2++)
				break;
		while (--length);
		return(length);
	}
	
    int auramain(void)
    {
		char *response = fgets("What is your name: ");
		printf("Your name is: ");
		printf(response);
		return 0;
    }
