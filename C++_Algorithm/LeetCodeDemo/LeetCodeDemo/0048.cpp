using namespace std;
#include <iostream>
#include <vector>
#include <string>

class Solution {
public:
    void rotate(vector<vector<int>>& a) {
        int n = a.size()-1;
        for (int i = 0; i <= (n-1)/2; i++)
        {
            for (int j = 0; j <= n/2; j++)
            {
                swap(a[i][j], a[j][n-i]);
                swap(a[i][j], a[n-i][n - j]);
                swap(a[i][j], a[n-j][i]);
            }
        }
    }
};