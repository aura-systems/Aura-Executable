#include "lib/stdio.h"

asm (".code32\n"
     "call __main\n"
	 "ret\n");

	
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
				 :
				 : "r"(returned)
				 : "edi" , "eax", "memory");

	return returned;
}


int _main(void)
{
	char *response = fgets("What is your name: ");
	printf("Hello ");
	printf(response);
	return 0;
}
