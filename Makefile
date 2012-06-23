all:
	MONO_IOMAP=case xbuild /verbosity:quiet /nologo

install:
	mkdir -p ~/MCForge
	mv -T "./MCForge 2.0/bin/Debug" ~/MCForge
