using namespace std;
#include <iostream>
#include <vector>
//设计题 两分 哈希
//遍历原数组，data[]用一个二位数组存储每个数出现的位置，两分查找。
class RangeFreqQuery {
private:
    vector<vector<int>> data{10001};
public:
    RangeFreqQuery(vector<int>& arr) {
        for (int i=0;i<arr.size();i++)
        {
            data[arr[i]].push_back(i);
        }
    }

    int query(int left, int right, int value) {
        auto l=lower_bound(data[value].begin(), data[value].end(),left);
        if (l == data[value].end())return 0;
        auto r = lower_bound(data[value].begin(), data[value].end(), right);
        if (r == data[value].end())return r-l;
        if(*r>right)return r - l;
        return r - l+1;
    }
};