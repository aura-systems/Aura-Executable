#include "stdio.h"
#include "stdarg.h"
#include "stdlib.h"

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