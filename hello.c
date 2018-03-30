asm (".code32\n"
     "call _auramain\n"
	 "ret\n");
	 
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
	
    int auramain(void)
    {
		char *response = fgets("What is your name: ");
		printf("Your name is: ");
		printf(response);
		return 0;
    }
