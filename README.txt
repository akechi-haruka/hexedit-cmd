hexedit-cmd
2021-2024 Haruka
Licensed under the Unlicense.

---

Command line hex-edit application.

Run as: hexedit.exe <file> <outfile> <offset> <tobyte1,tobyte2,...> [frombyte1,frombyte2,...]
file: input file
outfile: output file
offset: the offset in 0x1234 format
tobyte1..n: the bytes to patch (ex. 0x00,0x12,0x34,0x56)
frombyte1..n: optional, the bytes that are expected at this location, if they don't match the patch will fail.

Return codes:
0: success
7: already patched (success)
1: argument error
2: offset format error
3: tobytes parse error
4: frombytes parse error
5: input file does not exist
6: tobytes and frombytes are different length
8: byte not matching frombytes found