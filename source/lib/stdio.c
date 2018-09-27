#include "stdio.h"

int printf(const char *string, ...)
{
	asm volatile ("mov   $0xC, %%edx\n"
				 "mov   %0, %%ecx\n"
				 "mov   $0x1, %%ebx\n"
				 "mov   $0x4, %%eax\n"
				 "int   $0x48\n"
				 "mov   $0x1, %%eax\n"
				 "int   $0x48\n"
				 :
				 : "r"(string)
				 : "edx", "ebx", "eax" , "ecx", "memory");
	return 0;
}