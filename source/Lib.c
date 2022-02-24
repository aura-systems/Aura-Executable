#include "Lib.h"

int printf(const char *string, ...)
{
	asm volatile ("mov   $0x01, %%eax\n" //Print function
				 "mov   %0, %%esi\n"     //mov string content to ESI
				 "int   $0x48\n"         //interrupt 0x48 (Aura API)
				 :
				 : "r"(string)
				 : "esi" , "eax", "memory");
	return (0);
}