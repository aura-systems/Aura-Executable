#include "stdio.h"

asm (".code32\n"
     "call __main\n"
	 "ret\n");
	 
	
char *fgets(char *string)
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

void clear()
{
   asm volatile ("mov   $0x02, %eax\n" //clear function
				 "int   $0x48\n");		   
}

void setcursor(int x, int y)
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

void resetcursor()
{
   asm volatile ("mov   $0x31, %eax\n" //resetcursor function
				 "int   $0x48\n");
}

int _main(void)
{
	char *response = fgets("What is your name: ");
	printf("Hello ");
	printf(response);
	return 0;
}
