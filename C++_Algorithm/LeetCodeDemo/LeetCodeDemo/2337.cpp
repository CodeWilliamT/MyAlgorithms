using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//��ģ��
//��start����L��R��λ��sl,sr��target����L��R��λ��tl,tr;
//sl��tl��Ŀ���ȣ����У�sl[i]>=tl[i]��sr[i]<=tr[i];
//���Բ���¼��ֱ��һ�Զ�Ӧ��
class Solution {
public:
    bool canChange(string s, string t) {
        int n = s.size();
        int j = 0;
        for (int i = 0; i < n;i++) {
            if (s[i] == 'L') {
                while (j < n&&t[j] != 'L') {
                    if (t[j] == 'R')
                        return false;
                    j++;
                }
                if (i < j||j>=n)
                    return false;
                j++;
            }
            else if (s[i] == 'R') {
                while (j < n&&t[j] != 'R') {
                    if (t[j] == 'L')
                        return false;
                    j++;
                }
                if (i > j || j >= n)
                    return false;
                j++;
            }
        }
        while (j < n) {
            if (t[j] == 'L' || t[j] == 'R')
                return false;
            j++;
        }
        return true;
    }
};