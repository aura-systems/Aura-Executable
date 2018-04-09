#include "lib/stdlib.h"

int _main(void)
{
	char *response = fgets("What is your name: ");
	printf("Hello ");
	printf(response);
	return 0;
}
