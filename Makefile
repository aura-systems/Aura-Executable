SDIR	:=	source
ODIR	:=	build
CFILES	:=	$(wildcard $(SDIR)/*.c)
CC		:=	gcc
OBJCOPY	:=	objcopy
LD = ld.exe
OBJS	:=	$(patsubst $(SDIR)/%.c, $(ODIR)/%.o, $(CFILES))

CFLAGS = -std=gnu99 -Os -nostdlib -m32 -ffreestanding
LDFLAGS =  -Ttext=0x0

TARGET = $(shell basename $(CURDIR)).bin

$(TARGET): $(ODIR) $(OBJS)
	$(CC) -c $(CFLAGS) -o $(ODIR)/*.o $(CLFILES)
	$(LD) $(ODIR)/*.o $(SDIR)/linker/aura.ld -o $(ODIR)/aexe.tmp $(LDFLAGS)
	$(OBJCOPY) -O binary $(ODIR)/aexe.tmp $(ODIR)/program.aexe
	rm -f $(ODIR)/aexe.tmp
	
$(ODIR)/%.o: $(SDIR)/%.c
	$(CC) -c -o $@ $< $(CFLAGS)
	
$(ODIR):
	@mkdir $@
	
.PHONY: clean

clean:
	rm -f $(TARGET) $(ODIR)/*.o $(ODIR)/program.aexe