using namespace std;
#include <iostream>
#include <vector>
#include <string>
//��ϣ
//a=��������b���ڸ����ַ�����Ƶ��ƫС֮��-a
class Solution {
public:
    string getHint(string secret, string guess) {
        int a = 0, b = 0;
        int va[10]{}, vb[10]{};
        for (int i = 0; i < secret.size(); i++)
        {
            if (secret[i] == guess[i])a++;
            va[secret[i] - '0']++;
            vb[guess[i] - '0']++;
        }
        for (int i = 0; i < 10; i++)
        {
            b += min(va[i], vb[i]);
        }
        b-=a;
        return to_string(a) + "A" + to_string(b)+"B";
    }
};