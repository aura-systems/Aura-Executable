#include "Lib.h"

int printf(const char *string, ...)
{
	asm volatile ("mov   $0x01, %%eax\n" //Print function
				 "mov   %0, %%edi\n"     //mov string pointer to ESI
				 "int   $0x48\n"         //interrupt 0x48 (Aura API)
				 :
				 : "r"(string)
				 : "edi", "eax", "memory");
	return 1;
}

void clear()
{
	asm volatile ("mov   $0x02, %%eax\n" //Clear function
				 "int   $0x48\n"         //interrupt 0x48 (Aura API)
				 :
				 :
				 : "eax", "memory");
}