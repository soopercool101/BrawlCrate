#include <cstdio>
#include <cstring>

#include <fstream>
#include <iostream>

#include <windows.h>


using namespace std;

const char* brsarName = "smashbros_sound.brsar";
const char* spdFile = "sawnd.spd";
const char* sptFile = "sawnd.spt";

long long arg[8];

fstream Orig, Copy;

void CopyData(long long size)
{
    char* data = new char[size];
    printf("%lld\n", size);
    Orig.read(data, size);
    Copy.write(data, size);
    delete[] data;
}

void RemoveSpace(long long offset, long long size)
{
    printf("Removing bytes from brsar... Please wait.\n");
    printf("%lld offset.\n", offset);
    printf("%lld bytes to alter.\n", size);

    Orig.open(brsarName, ios::in | ios::out | ios::binary);
    Copy.open("temp.brsar", ios::in | ios::out | ios::app | ios::binary);
    Orig.seekg(0, ios::end);
    long long brsarsize = Orig.tellg();

    Orig.seekg(0, ios::beg);
    Copy.seekg(0, ios::beg);

    CopyData(offset);

    Orig.seekg(offset + size, ios::beg);

    CopyData(brsarsize - offset - size);

    Orig.close();
    Copy.close();

    remove(brsarName);
    std::rename("temp.brsar", brsarName);
    //system("copy temp.brsar smashbros_sound.brsar");
    //remove("temp.brsar");
}

void EmptySpace(long long offset, long long size)
{
    printf("Adding empty space to the brsar... Please wait.\n");
    printf("%lld offset.\n", offset);
    printf("%lld bytes to alter.\n", size);

    Orig.open(brsarName, ios::in | ios::out | ios::binary);
    Copy.open("temp.brsar", ios::in | ios::out | ios::app | ios::binary);
    Orig.seekg(0, ios::end);
    long long brsarsize = Orig.tellg();

    Orig.seekg(0, ios::beg);
    Copy.seekg(0, ios::beg);

    CopyData(offset);
    printf("__________\n");
    int prevPercent = 0;
    for (long long i = 0; i < size; i++)
    {
        const int percent = i * 100 / size;
        if (percent >= prevPercent + 10)
        {
            printf("#");
            prevPercent = percent;
        }
        Copy.put(0);
    }
    printf("\n");

    CopyData(brsarsize - offset);

    Orig.close();
    Copy.close();

    remove(brsarName);
    std::rename("temp.brsar", brsarName);
}

int readint()
{
    int a = Orig.get();
    int b = Orig.get();
    int c = Orig.get();
    int d = Orig.get();
    return a * 256 * 256 * 256 + b * 256 * 256 + c * 256 + d;
}

int readintc()
{
    int a = Copy.get();
    int b = Copy.get();
    int c = Copy.get();
    int d = Copy.get();
    return a * 256 * 256 * 256 + b * 256 * 256 + c * 256 + d;
}

void go(int a)
{
    Orig.seekg(a);
    Orig.seekp(a);
}

void writeint(int a)
{
    Orig.seekp(Orig.tellg());
    Orig.put(a >> 24);
    Orig.put(a >> 16);
    Orig.put(a >> 8);
    Orig.put(a);
}

void writeintc(int a)
{
    Copy.put(a >> 24);
    Copy.put(a >> 16);
    Copy.put(a >> 8);
    Copy.put(a);
}

int sign(int a)
{
    if (a < 0)
    {
        return -1;
    }
    if (a > 0)
    {
        return 1;
    }
    return 0;
}

void SawndInsert2()
{
    unsigned long long group_offset = 0;
    unsigned long long samples;
    long long grak = 0;

    Copy.open("sawnd.sawnd", ios::in | ios::out | ios::binary);
    if (!Copy.good())
    {
        printf("Error Opening sawnd.sawnd\n");
        printf("%s", strerror(errno));
        return;
    }
    Copy.seekg(1);
    int groupe = readintc();
    printf("target group: %d\n", groupe);
    Copy.seekg(5);
    unsigned long long total_size = readintc();
    // Find the group data.
    Orig.open(brsarName, ios::in | ios::out | ios::binary);
    if (!Orig.good())
    {
        printf("Error Opening brsar file: %s\n", brsarName);
        printf("%s", strerror(errno));
        return;
    }
    Orig.seekg(24);
    unsigned long long address = readint();
    unsigned long long info_address = address;
    unsigned long long relocation_address = info_address + 44;
    // Go to the offset to Group Relocation Group (relative to 0x08 in INFO Header)
    Orig.seekg(relocation_address, ios::beg);
    address = readint();
    // Read number of entries
    Orig.seekg(info_address + 8 + address, ios::beg);
    unsigned long long group_num = readint();
    unsigned long long grprel_baseoff = info_address + address + 16;
    // Find the group.
    unsigned long long grid = -1;
    for (int i = 0; i < group_num; i++)
    {
        Orig.seekg(grprel_baseoff + i * 8, ios::beg);
        address = readint();
        Orig.seekg(info_address + 8 + address, ios::beg);
        grid = readint();
        if (grid == groupe)
        {
            grak = i;
            printf("%llu\n", grid);
            group_offset = info_address + 8 + address;
            break;
        }
    }
    if (grid == -1)
    {
        printf("ERROR: The group is incorrect. There may be something wrong with your .sawnd file.\n");
        Copy.close();
        Orig.close();
        remove("sawnd.sawnd");
        Sleep(4000);
        return;
    }
    go(group_offset + 16);
    unsigned long long base_rwsd = readint();
    go(group_offset + 28);
    unsigned long long old_size = readint();
    long long size_difference = total_size - old_size;
    // Pick the collection relocation group address
    go(info_address + 36);
    address = readint();
    unsigned long long collect_rel = info_address + 8 + address;
    // -------MODIFYING-------
    // First, collections as subgroups and collections.
    go(group_offset + 36);
    address = readint();
    unsigned long long col_rel = address + info_address + 8;
    go(col_rel);
    unsigned long long col_num = readint();
    printf("Modifying Collections\n");
    for (int i = 0; i < col_num; i++)
    {
        Copy.seekg(17 + 12 * i);
        unsigned long long l2 = readintc();
        Copy.seekg(13 + 12 * i);
        unsigned long long o2 = readintc();

        go(col_rel + 8 + 8 * i);
        address = readint();
        go(info_address + 8 + address);
        unsigned long long col_id = readint();
        go(collect_rel + 8 + 8 * col_id);
        address = readint();
        go(info_address + 8 + address + 4);
        writeint(l2);

        go(col_rel + 8 * i + 8);
        address = readint();
        go(info_address + 8 + address + 12);
        writeint(o2);
        go(info_address + 8 + address + 16);
        writeint(l2);
    }
    // Second, in the RSAR header.
    Orig.seekg(36, ios::beg);
    address = readint();
    address += size_difference;
    Orig.seekg(36, ios::beg);
    writeint(address);
    printf("Modifying Groups\n");
    // Now groups.
    for (int i = grak + 1; i < group_num; i++)
    {
        go(grprel_baseoff + i * 8);
        address = readint();
        go(info_address + 8 + address + 16);
        samples = readint();
        samples += size_difference;
        go(info_address + 8 + address + 16);
        writeint(samples);
        go(info_address + 8 + address + 24);
        samples = readint();
        samples += size_difference;
        go(info_address + 8 + address + 24);
        writeint(samples);
    }
    // The group itself.
    go(group_offset + 28);
    samples = readint();
    samples += size_difference;
    go(group_offset + 28);
    writeint(samples);
    // Modify stuff after FILE header.
    go(32);
    address = readint();
    go(address + 4);
    samples = readint();
    samples += size_difference;
    go(address + 4);
    writeint(samples);
    // Data copy.
    printf("Copying\n");
    Copy.close();
    Orig.close();
    if (size_difference < 0)
    {
        RemoveSpace(base_rwsd, -size_difference);
    }
    if (size_difference > 0)
    {
        EmptySpace(base_rwsd, size_difference);
    }
    Orig.open("sawnd.sawnd", ios::in | ios::out | ios::binary);
    Copy.open(brsarName, ios::in | ios::out | ios::binary);
    Orig.seekg(0, ios::end);
    long long edn = Orig.tellg();
    Copy.seekp(base_rwsd);
    Orig.seekg(9 + col_num * 12);
    for (long long i = 9 + col_num * 12; i < edn; i++)
    {
        int o = Orig.get();
        Copy.put(o);
    }
    printf("Done.");
    Sleep(50);
}

void SawndInsert1()
{
    unsigned long long group_offset = 0;
    unsigned long long samples;
    long long grak = 0;

    Copy.open("sawnd.sawnd", ios::in | ios::out | ios::binary);
    Copy.seekg(1);
    int groupe = readintc();
    printf("target group: %d\n", groupe);
    Copy.seekg(5);
    unsigned long long total_size = readintc();
    // Find the group data.
    Orig.open(brsarName, ios::in | ios::out | ios::binary);
    Orig.seekg(24);
    unsigned long long address = readint();
    unsigned long long info_address = address;
    unsigned long long relocation_address = info_address + 44;
    // Go to the offset to Group Relocation Group (relative to 0x08 in INFO Header)
    Orig.seekg(relocation_address, ios::beg);
    address = readint();
    // Read number of entries
    Orig.seekg(info_address + 8 + address, ios::beg);
    unsigned long long group_num = readint();
    unsigned long long grprel_baseoff = info_address + address + 16;
    // Find the group.
    unsigned long long grid = -1;
    for (int i = 0; i < group_num; i++)
    {
        Orig.seekg(grprel_baseoff + i * 8, ios::beg);
        address = readint();
        Orig.seekg(info_address + 8 + address, ios::beg);
        grid = readint();
        if (grid == groupe)
        {
            grak = i;
            printf("%llu\n", grid);
            group_offset = info_address + 8 + address;
            break;
        }
    }
    if (grid == -1)
    {
        printf("ERROR: The group is incorrect. There may be something wrong with your .sawnd file.\n");
        Copy.close();
        Orig.close();
        remove("sawnd.sawnd");
        Sleep(4000);
        return;
    }
    go(group_offset + 16);
    unsigned long long base_rwsd = readint();
    go(group_offset + 28);
    unsigned long long old_size = readint();
    long long size_difference = total_size - old_size;
    // Pick the collection relocation group address
    go(info_address + 36);
    address = readint();
    unsigned long long collect_rel = info_address + 8 + address;
    // -------MODIFYING-------
    // First, collections as subgroups and collections.
    go(group_offset + 36);
    address = readint();
    unsigned long long col_rel = address + info_address + 8;
    go(col_rel);
    unsigned long long col_num = readint();
    for (int i = 0; i < col_num; i++)
    {
        Copy.seekg(17 + 12 * i);
        unsigned long long l2 = readintc();
        Copy.seekg(13 + 12 * i);
        unsigned long long o2 = readintc();

        go(col_rel + 8 + 8 * i);
        address = readint();
        go(info_address + 8 + address);
        unsigned long long col_id = readint();
        go(collect_rel + 8 + 8 * col_id);
        address = readint();
        go(info_address + 8 + address + 4);
        writeint(l2);

        go(col_rel + 8 * i + 8);
        address = readint();
        go(info_address + 8 + address + 12);
        writeint(o2);
        go(info_address + 8 + address + 16);
        writeint(l2);
    }
    // Second, in the RSAR header.
    Orig.seekg(36, ios::beg);
    address = readint();
    address += size_difference;
    Orig.seekg(36, ios::beg);
    writeint(address);
    // Now groups.
    for (int i = grak + 1; i < group_num; i++)
    {
        go(grprel_baseoff + i * 8);
        address = readint();
        go(info_address + 8 + address + 16);
        samples = readint();
        samples += size_difference;
        go(info_address + 8 + address + 16);
        writeint(samples);
        go(info_address + 8 + address + 24);
        samples = readint();
        samples += size_difference;
        go(info_address + 8 + address + 24);
        writeint(samples);
    }
    // The group itself.
    go(group_offset + 28);
    samples = readint();
    samples += size_difference;
    go(group_offset + 28);
    writeint(samples);
    // Modify stuff after FILE header.
    go(32);
    address = readint();
    go(address + 4);
    samples = readint();
    samples += size_difference;
    go(address + 4);
    writeint(samples);
    // Data copy.
    Copy.close();
    Orig.close();
    if (size_difference < 0)
    {
        RemoveSpace(base_rwsd, -size_difference);
    }
    if (size_difference > 0)
    {
        EmptySpace(base_rwsd, size_difference);
    }
    Orig.open("sawnd.sawnd", ios::in | ios::out | ios::binary);
    Copy.open(brsarName, ios::in | ios::out | ios::binary);
    Copy.seekp(base_rwsd);
    Orig.seekg(9 + col_num * 12);
    for (long long i = 0; i < total_size; i++)
    {
        int o = Orig.get();
        Copy.put(o);
    }
    printf("Done.");
    Sleep(50);
}

void Sawnd()
{
    printf("Inserting the .sawnd file... Please wait.\n");

    Copy.open("sawnd.sawnd", ios::in | ios::out | ios::binary);
    char a = Copy.get();
    if (a != 1 && a != 2)
    {
        printf("This version of .sawnd file is not supported.");
        Sleep(1000);
    }
    Copy.close();
    if (a == 1)
    {
        SawndInsert1();
    }
    if (a == 2)
    {
        SawndInsert2();
    }
}

void SawndCreate(long long group)
{
    printf("Creating the .sawnd file... Please wait.\n");
    unsigned long long group_offset = 0;

    Copy.open("sawnd.sawnd", ios::in | ios::out | ios::binary | ios::app);
    if (!Copy.good())
    {
        printf("Error creating sawnd.sawnd\n");
        printf("%s", strerror(errno));
        return;
    }
    Copy.put(2);
    writeintc(group);
    // Find the group data.
    Orig.open(brsarName, ios::in | ios::out | ios::binary);
    if (!Orig.good())
    {
        printf("Error Opening BRSAR %s\n", brsarName);
        printf("%s", strerror(errno));
        return;
    }
    Orig.seekg(24);
    unsigned long long address = readint();
    unsigned long long info_address = address;
    unsigned long long relocation_address = info_address + 44;
    // Go to the offset to Group Relocation Group (relative to 0x08 in INFO Header)
    Orig.seekg(relocation_address, ios::beg);
    address = readint();
    // Read number of entries
    Orig.seekg(info_address + 8 + address, ios::beg);
    unsigned long long group_num = readint();
    unsigned long long grprel_baseoff = info_address + address + 16;
    // Find the group.
    unsigned long long grid = -1;
    for (int i = 0; i < group_num; i++)
    {
        Orig.seekg(grprel_baseoff + i * 8, ios::beg);
        address = readint();
        Orig.seekg(info_address + 8 + address, ios::beg);
        grid = readint();
        if (grid == group)
        {
            printf("%llu\n", grid);
            group_offset = info_address + 8 + address;
            break;
        }
    }
    if (grid == -1)
    {
        printf("ERROR: The group is incorrect.\n");
        Copy.close();
        Orig.close();
        remove("sawnd.sawnd");
        Sleep(4000);
        return;
    }
    go(group_offset + 16);
    unsigned long long base_rwsd = readint();
    go(group_offset + 20);
    unsigned long long total_size = 0;
    total_size += readint();
    go(group_offset + 28);
    total_size += readint();
    go(group_offset + 28);
    unsigned long long samples = readint();
    writeintc(samples);
    go(group_offset + 36);
    address = readint();
    unsigned long long col_rel = info_address + 8 + address;
    go(col_rel);
    unsigned long long col_num = readint();
    for (int i = 0; i < col_num; i++)
    {
        go(col_rel + 8 + i * 8);
        address = readint();
        go(info_address + 8 + address);
        samples = readint();
        writeintc(samples);
        go(info_address + 8 + address + 12);
        samples = readint();
        writeintc(samples);
        go(info_address + 8 + address + 16);
        samples = readint();
        writeintc(samples);
    }
    go(base_rwsd);
    printf("Size: %llu\n", total_size);
    Sleep(500);
    for (long long i = 0; i < total_size; i++)
    {
        int o = Orig.get();
        Copy.put(o);
    }
    Copy.close();
    Orig.close();
}

void Hex(long long group)
{
    printf("Inserting the hex packet... Please wait.\n");

    unsigned long long group_offset = 0;

    Orig.open(brsarName, ios::in | ios::out | ios::binary);
    Orig.seekg(24);
    unsigned long long address = readint();
    unsigned long long info_address = address;
    unsigned long long relocation_address = info_address + 44;
    // Go to the offset to Group Relocation Group (relative to 0x08 in INFO Header)
    Orig.seekg(relocation_address, ios::beg);
    address = readint();
    // Read number of entries
    Orig.seekg(info_address + 8 + address, ios::beg);
    unsigned long long group_num = readint();
    unsigned long long grprel_baseoff = info_address + address + 16;
    // Find the group.
    unsigned long long grid = -1;
    for (int i = 0; i < group_num; i++)
    {
        Orig.seekg(grprel_baseoff + i * 8, ios::beg);
        address = readint();
        Orig.seekg(info_address + 8 + address, ios::beg);
        grid = readint();
        if (grid == group)
        {
            printf("%llu\n", grid);
            group_offset = info_address + 8 + address;
            break;
        }
    }
    if (grid == -1)
    {
        printf(
            "ERROR: The group is incorrect.\nProcess stopped, no changes were done to brsar, you may use it again without loading backup.");
        Orig.close();
        Sleep(4000);
        return;
    }
    Orig.seekg(group_offset + 16);
    unsigned long long base_rwsd = readint();

    Copy.open("hex.hex", ios::in | ios::out | ios::binary);
    Copy.seekg(0, ios::end);
    unsigned long long size = Copy.tellg();
    Copy.seekg(0, ios::beg);
    char* data = new char[10240];
    Orig.seekp(base_rwsd);
    unsigned long long progress = 0;
    while (progress < size - 10240)
    {
        Copy.read(data, 10240);
        Orig.write(data, 10240);
        progress += 10240;
    }
    while (progress < size)
    {
        Copy.read(data, 1);
        Orig.write(data, 1);
        progress++;
    }
    delete[] data;
    Copy.close();
    Orig.close();
}

void Insert(long long group, long long collection, long long wave, int frequency, int basewave, int loop)
{
    printf("Inserting the sound... Please wait.\nfrequency %d\n", frequency);
    if (basewave != -1)
    {
        printf("base wave: %d\n", basewave);
    }
    if (loop == 0)
    {
        printf("nonlooping sample.");
    }
    else
    {
        printf("LOOPING sample.");
    }

    char* data;
    int col_ide;
    unsigned long long address;
    unsigned long long info_address;
    unsigned long long relocation_address;
    unsigned long long group_num;
    unsigned long long grprel_baseoff;
    unsigned long long grid;
    unsigned long long group_offset = 0;
    unsigned long long col_num;
    unsigned long long col_rel;
    unsigned long long col_id;
    unsigned long long col_offset;
    unsigned long long base_rwsd;
    unsigned long long base_data;
    unsigned long long rwsd_offset;
    unsigned long long rwsd_length;
    unsigned long long data_offset;
    unsigned long long data_length;
    unsigned long long WAVE_offset;
    unsigned long long wave_num;
    unsigned long long wave_offset;
    unsigned long long samples;
    unsigned long long old_size;
    unsigned long long new_size;
    unsigned long long grak;
    unsigned long long a;
    unsigned long long basewave_offset;
    unsigned long long mydata;
    long long size_difference;
    long long org_size_difference;


    // FIRST: Read the number of bytes of the sound.
    Copy.open(spdFile, ios::in | ios::out | ios::binary);
    Copy.seekg(0, ios::end);
    new_size = Copy.tellg();
    printf("new size inst %llu\n", new_size);
    Copy.close();
    if (new_size < 500)
    {
        printf("ERROR: Size of sawnd.spd file is too small.\nTry using a bigger sound file.\n");
        Sleep(2000);
        return;
    }
    // SECOND: Calculate the byte difference.
    // Open the brsar.
    Orig.open(brsarName, ios::in | ios::out | ios::binary);

    if (!Orig.good())
    {
        printf("Error Opening BRSAR\n");
        printf("%s", strerror(errno));
        return;
    }

    // Get the offset of INFO header.
    Orig.seekg(24);
    address = readint();
    info_address = address;
    relocation_address = info_address + 44;
    // Go to the offset to Group Relocation Group (relative to 0x08 in INFO Header)
    Orig.seekg(relocation_address, ios::beg);
    address = readint();
    // Read number of entries
    Orig.seekg(info_address + 8 + address, ios::beg);
    group_num = readint();
    grprel_baseoff = info_address + address + 16;
    // Find the group.
    grid = -1;
    for (int i = 0; i < group_num; i++)
    {
        Orig.seekg(grprel_baseoff + i * 8, ios::beg);
        address = readint();
        Orig.seekg(info_address + 8 + address, ios::beg);
        grid = readint();
        if (grid == group)
        {
            grak = i;
            printf("%llu\n", grid);
            group_offset = info_address + 8 + address;
            break;
        }
    }
    if (grid == -1)
    {
        printf(
            "ERROR: The group is incorrect.\nProcess stopped, no changes were done to brsar, you may use it again without loading backup.");
        Orig.close();
        Sleep(4000);
        return;
    }
    Orig.seekg(group_offset + 16);
    base_rwsd = readint();
    Orig.seekg(group_offset + 24);
    base_data = readint();
    // Get the number of subgroups belonging to the group.
    Orig.seekg(group_offset + 36, ios::beg);
    address = readint();
    Orig.seekg(info_address + 8 + address, ios::beg);
    col_num = readint();
    if (col_num == 0)
    {
        printf(
            "ERROR: The group has no collections.\nProcess stopped, no changes were done to brsar, you may use it again without loading backup.");
        Orig.close();
        Sleep(4000);
        return;
    }
    // Find the subgroup.
    col_rel = info_address + 16 + address;
    col_id = -1;
    for (int i = 0; i < col_num; i++)
    {
        Orig.seekg(col_rel + 8 * i, ios::beg);
        address = readint();
        Orig.seekg(info_address + 8 + address, ios::beg);
        col_id = readint();
        if (col_id == collection)
        {
            col_ide = i;
            break;
        }
    }
    if (col_id == -1)
    {
        printf(
            "ERROR: The group does not contain a collection with provided number.\nProcess stopped, no changes were done to brsar, you may use it again without loading backup.");
        Orig.close();
        Sleep(4000);
        return;
    }
    // Pick the data from the subgroup.
    col_offset = info_address + 8 + address;
    Orig.seekg(col_offset + 4);
    rwsd_offset = readint();
    Orig.seekg(col_offset + 8);
    rwsd_length = readint();
    Orig.seekg(col_offset + 12);
    data_offset = readint();
    Orig.seekg(col_offset + 16);
    data_length = readint();
    // Find the WAVE header.
    Orig.seekg(base_rwsd + rwsd_offset + 24);
    printf("%llu\n", rwsd_offset);
    address = readint();
    WAVE_offset = base_rwsd + rwsd_offset + address;
    // Count WAVEs.
    Orig.seekg(WAVE_offset + 8);
    wave_num = readint();
    if (wave_num < wave)
    {
        printf(
            "ERROR: The chosen wave doesn't exist in the subgroup.\nProcess stopped, no changes were done to brsar, you may use it again without loading backup.");
        Orig.close();
        Sleep(4000);
        return;
    }
    // Get to the Wave data itself.
    Orig.seekg(WAVE_offset + 12 + wave * 4);
    address = readint();
    wave_offset = WAVE_offset + address;
    // Now, pick the number of samples.
    // Single byte of data contains 2 samples.
    // Samples number divided by 2 and rounded up next is the byte number.
    Orig.seekg(wave_offset + 12);
    samples = readint();
    if (samples / 2 * 2 == samples)
    {
        old_size = samples / 2;
    }
    else
    {
        old_size = samples / 2 + 1;
    }
    size_difference = new_size - old_size;
    printf("new size: %llu\n", new_size);
    printf("old size: %llu\n", old_size);
    printf("size difference: %lld\n", size_difference);
    if (size_difference % 16 != 0)
    {
        if (size_difference < 0)
        {
            size_difference = size_difference / 16 * 16;
        }
        else
        {
            size_difference = size_difference / 16 * 16 + 16;
        }
    }
    printf("size difference: %lld\n", size_difference);
    org_size_difference = size_difference;
    Orig.seekg(wave_offset + 20);
    address = readint() + base_data + data_offset;
    Orig.close();
    if (size_difference < 0)
    {
        RemoveSpace(address, -size_difference);
    }
    else
    {
        EmptySpace(address, size_difference);
    }
    //printf("Opening BRSAR\n");
    Orig.open(brsarName, ios::in | ios::out | ios::binary);
    //printf("Opened BRSAR\n");
    samples = size_difference + size_difference % 16;
    size_difference = samples;
    org_size_difference = samples;
    // There it is.
    // THIRD: Mass offset/length modifying.
    // First, in the RSAR header.
    //printf("Modifying RSAR header\n");
    Orig.seekg(36, ios::beg);
    address = readint();
    address += size_difference;
    Orig.seekg(36, ios::beg);
    writeint(address);

    // Now, onto collections.
    //printf("Modifying Collection\n");
    Orig.seekg(info_address + 36);
    address = readint();
    Orig.seekg(info_address + address + 16 + collection * 8);
    address = readint();
    Orig.seekg(info_address + address + 12);
    samples = readint();
    samples += size_difference;
    Orig.seekg(info_address + address + 12);
    writeint(samples);
    // Now groups.
    // printf("Looking for group\n");
    for (int i = grak + 1; i < group_num; i++)
    {
        go(grprel_baseoff + i * 8);
        address = readint();
        go(info_address + 8 + address + 16);
        samples = readint();
        samples += size_difference;
        go(info_address + 8 + address + 16);
        writeint(samples);
        go(info_address + 8 + address + 24);
        samples = readint();
        samples += size_difference;
        go(info_address + 8 + address + 24);
        writeint(samples);
    }
    // The group itself.
    // printf("Modifying Group\n");
    go(group_offset + 28);
    samples = readint();
    samples += size_difference;
    go(group_offset + 28);
    writeint(samples);
    // The collection.
    // printf("Modifying Collection\n");
    go(col_offset + 16);
    samples = readint();
    samples += size_difference;
    go(col_offset + 16);
    writeint(samples);
    // The other collections.
    for (int i = col_ide + 1; i < col_num; i++)
    {
        Orig.seekg(col_rel + 8 * i, ios::beg);
        address = readint();
        address += info_address + 8;

        go(address + 4);
        samples = readint();
        samples += size_difference;
        go(address + 4);
        //writeint(samples);
        go(address + 12);
        samples = readint();
        samples += size_difference;
        go(address + 12);
        writeint(samples);
    }
    // WAVE.
    // printf("Modifying Wav Header\n");
    size_difference = org_size_difference;
    for (int i = 0; i < wave_num; i++)
    {
        go(WAVE_offset + 12 + i * 4);
        address = readint();
        if (i == basewave)
        {
            basewave_offset = WAVE_offset + address;
        }
        if (i < wave + 1)
        {
            continue;
        }
        go(WAVE_offset + address + 20);
        samples = readint();
        samples += size_difference;
        go(WAVE_offset + address + 20);
        writeint(samples);
    }
    // Paste the sound data.

    Orig.seekp(wave_offset + 20);
    samples = readint();
    mydata = samples;
    Orig.seekp(base_data + data_offset + samples);
    Copy.open(spdFile, ios::in | ios::out | ios::binary);
    if (!Copy.good())
    {
        printf("Error Opening sawnd.spd\n");
        printf("%s", strerror(errno));
        return;
    }
    //  printf("Opened Sawnd.spd\n");
    Copy.seekp(0, ios::end);
    samples = Copy.tellp();
    Copy.seekp(0, ios::beg);
    data = new char[256];
    address = 0;
    unsigned long long prevPercentCompleted = 10;
    printf("Copying Sound\n");
    printf("___________\n");
    while (address < samples - 256)
    {
        //printf("address = %d", address);
        unsigned long long percentCompleted = address * 100 / samples;
        if (percentCompleted >= prevPercentCompleted + 10)
        {
            printf("#");
            prevPercentCompleted = percentCompleted;
        }
        //    printf("about to read Copy\n");
        Copy.read(data, 256);
        //printf("about to write Data\n");
        Orig.write(data, 256);
        //printf("Wrote Data\n");
        address += 256;
    }
    printf("\n");
    while (address < samples)
    {
        Copy.read(data, 1);
        Orig.write(data, 1);
        address++;
    }
    delete[] data;
    Copy.close();
    // Base wave insertion.
    data = new char[108];
    if (basewave != -1)
    {
        //printf("basewave %d\nbasewave %lld\n    wave %lld\n",basewave,basewave_offset,wave_offset);
        //Sleep(2000);
        go(basewave_offset);
        Orig.read(data, 108);
        go(wave_offset);
        Orig.write(data, 108);
        go(wave_offset + 20);
        writeint(mydata);
    }
    delete[] data;
    // WAVE parameters.
    go(wave_offset + 2);
    if (loop != 0)
    {
        Orig.put(0);
    }
    else
    {
        Orig.put(1);
    }
    go(wave_offset + 3);
    printf("wave %llu\n", wave_offset);
    Sleep(2000);
    Orig.seekp(wave_offset + 3);
    Orig.put(frequency >> 16);
    Orig.put(frequency >> 8);
    Orig.put(frequency);
    go(wave_offset + 12);
    writeint(new_size * 2);
    // Paste the fix.
    Orig.seekp(wave_offset + 60);
    Copy.open(sptFile, ios::in | ios::out | ios::binary);
    Copy.seekg(32, ios::beg);
    printf("fix\n");
    data = new char[32];
    Copy.read(data, 32);
    printf("fix\n");
    Orig.write(data, 32);
    delete[] data;
    printf("fix\n");
    Copy.seekg(66, ios::beg);
    data = new char[2];
    Copy.read(data, 2);
    Orig.seekp(wave_offset + 94);
    Orig.write(data, 2);
    Orig.seekp(wave_offset + 100);
    Orig.write(data, 2);
    delete[] data;
    Copy.close();
    // Modify stuff after FILE header.
    go(32);
    address = readint();
    go(address + 4);
    samples = readint();
    samples += size_difference;
    go(address + 4);
    writeint(samples);

    Orig.close();
    printf("Done.");
    Sleep(100);
}

int main(int argc, char** argv)
{
    setbuf(stdout, nullptr);
    Orig.exceptions(ios::badbit);
    Copy.exceptions(ios::badbit);
    printf("Sawndz 0.13\n2010-2011 Jaklub\n2012 Agoaj\n\nspecial thanks to mastaklo, ssbbtailsfan, stickman, VILE\n");
    try
    {
        if (argc == 1)
        {
            printf("Please run through BrawlCrate.\n");
            Sleep(1000);
            return 0;
        }
        if (strcmp("emptyspace", argv[1]) == 0)
        {
            if (argc != 4)
            {
                printf(
                    "Incorrect number of arguments.\nemptyspace command requires 2 arguments.\noffset\nnumber of bytes");
                Sleep(1000);
                return 0;
            }
            sscanf(argv[2], "%lld", &arg[0]);
            sscanf(argv[3], "%lld", &arg[1]);
            EmptySpace(arg[0], arg[1]);
            return 0;
        }
        if (strcmp("removespace", argv[1]) == 0)
        {
            if (argc != 4)
            {
                printf(
                    "Incorrect number of arguments.\nemptyspace command requires 2 arguments.\noffset\nnumber of bytes");
                Sleep(1000);
                return 0;
            }
            sscanf(argv[2], "%lld", &arg[0]);
            sscanf(argv[3], "%lld", &arg[1]);
            RemoveSpace(arg[0], arg[1]);
            return 0;
        }
        if (strcmp("insert", argv[1]) == 0)
        {
            if (argc != 7 && argc != 8 && argc != 9)
            {
                printf("You supplied %d arguments\n", argc);
                printf(
                    "Incorrect number of arguments.\ninsert command requires 4 arguments.\ngroup id\ncollection id\nwave id\nfrequency\nlooping (0 - not loop, any other - loop)\nOPTIONAL\nBRSAR File\n .spd File");
                Sleep(1000);
                return 0;
            }
            sscanf(argv[2], "%lld", &arg[0]);
            sscanf(argv[3], "%lld", &arg[1]);
            sscanf(argv[4], "%lld", &arg[2]);
            sscanf(argv[5], "%lld", &arg[3]);
            sscanf(argv[6], "%lld", &arg[4]);
            if (argc > 7)
            {
                brsarName = argv[7];
            }
            // Originally sets sound filename, which isn't necessary using Super Sawndz implementation. Used depreciated functionality. if (argc > 8) { setSoundFileName(argv[8]); }
            Insert(arg[0], arg[1], arg[2], arg[3], -1, arg[4]);
            return 0;
        }
        if (strcmp("baseinsert", argv[1]) == 0)
        {
            if (argc != 8)
            {
                printf(
                    "Incorrect number of arguments.\nbaseinsert command requires 5 arguments.\ngroup id\ncollection id\nwave id\nfrequency\nlooping (0 - not loop, any other - loop)\nbase wave id");
                Sleep(1000);
                return 0;
            }
            sscanf(argv[2], "%lld", &arg[0]);
            sscanf(argv[3], "%lld", &arg[1]);
            sscanf(argv[4], "%lld", &arg[2]);
            sscanf(argv[5], "%lld", &arg[3]);
            sscanf(argv[6], "%lld", &arg[4]);
            sscanf(argv[7], "%lld", &arg[5]);
            Insert(arg[0], arg[1], arg[2], arg[3], arg[4], arg[5]);
            return 0;
        }
        if (strcmp("hex", argv[1]) == 0)
        {
            if (argc != 3)
            {
                printf("Incorrect number of arguments.\nhex command requires 1 argument.\ngroup id");
                Sleep(1000);
                return 0;
            }
            sscanf(argv[2], "%lld", &arg[0]);
            Hex(arg[0]);
            return 0;
        }
        if (strcmp("sawndcreate", argv[1]) == 0)
        {
            if (argc != 3 && argc != 4)
            {
                printf(
                    "Incorrect number of arguments.\nsawndcreate command requires 1 argument.\ngroup id\nOPTIONAL\nbrsar filename\n");
                Sleep(1000);
                return 0;
            }
            sscanf(argv[2], "%lld", &arg[0]);
            if (argc == 4)
            {
                brsarName = argv[3];
            }
            SawndCreate(arg[0]);
            return 0;
        }
        if (strcmp("sawnd", argv[1]) == 0)
        {
            if (argc == 3)
            {
                brsarName = argv[2];
            }
            Sawnd();
            return 0;
        }
        printf("Incorrect command.\n");
        Sleep(1000);
        return 0;
    }
    catch (std::exception &e)
    {
        printf("EXCEPTION");
        if (errno)
        {
            printf("%s", strerror(errno));
        }
        printf("%s", e.what());
        Sleep(1000);
    }
    return 0;
}
