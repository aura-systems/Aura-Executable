#include "stdio.h"

int printf(const char *string, ...)
{
	asm volatile ("mov   $0x01, %%eax\n" //Print function
				 "mov   %0, %%esi\n"
				 "int   $0x48\n"
				 :
				 : "r"(string)
				 : "esi" , "eax", "memory");
	return 0;
}

char *fgets(char *string)
{
	asm volatile ("mov   $0x02, %%eax\n" //Print function
				 "mov   %0, %%esi\n"
				 "int   $0x48\n"
				 :
				 : "r"(string)
				 : "esi" , "eax", "memory");

	char *returned;

	asm volatile ("mov   %%edi, %0\n"
				 "mov %%edi, 0x00\n"
				 :
				 : "r"(returned)
				 : "edi" , "eax", "memory");

	return returned;
}
