SDIR	:=	source
ODIR	:=	build
CFILES	:=	$(wildcard $(SDIR)/*.c)
CLIBFILES	:=	$(wildcard $(SDIR)/lib/*.c)
CC		:=	gcc
OBJCOPY	:=	objcopy
LD = ld.exe
OBJS	:=	$(patsubst $(SDIR)/%.c, $(ODIR)/%.o, $(CFILES))

CFLAGS = -std=gnu99 -Os -nostdlib -m32 -ffreestanding
LDFLAGS =  -Ttext=0x0

TARGET = $(shell basename $(CURDIR)).bin

$(TARGET): $(ODIR) $(OBJS)
	$(CC) -c $(CFLAGS) -o $(ODIR)/*.o $(CFILES)
	$(CC) -c $(CFLAGS) -o $(ODIR)/*.o $(CLIBFILES)
	$(LD) $(ODIR)/*.o $(SDIR)/linker/aura.ld -o $(ODIR)/program.tmp $(LDFLAGS)
	$(OBJCOPY) -O binary $(ODIR)/program.tmp $(ODIR)/program.bin
	rm -f $(ODIR)/program.tmp

$(ODIR)/%.o: $(SDIR)/%.c
	$(CC) -c -o $@ $< $(CFLAGS)

$(ODIR)/%.o: $(SDIR)/lib/%.c
	$(CC) -c -o $@ $< $(CFLAGS)

$(ODIR):
	@mkdir $@

.PHONY: clean

clean:
	rm -f $(TARGET) $(ODIR)/*.o $(ODIR)/program.bin
