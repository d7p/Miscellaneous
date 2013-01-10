#include <iostream>
#include <string>

using namespace std;

int main()
{
    const char * str = "3s 5h 6g";
    string cards[3];
    cards[0] = str[0]+str[1];
    cards[1] = str[3]+str[4];
    cards[2] = str[6]+str[7];

    printf("%s\n",cards[0]);
    printf("%s\n",cards[1]);
    printf("%s\n",cards[2]);
    return 0;
}
