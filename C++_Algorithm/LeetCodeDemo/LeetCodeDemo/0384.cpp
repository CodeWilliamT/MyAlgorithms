using namespace std;
#include <vector>

//设计题
//每次从老序列随机抽取一个加到新序列中。
class Solution {
private:
    vector<int> data;
    vector<int> databak;
public:
    Solution(vector<int>& nums) {
        data = nums;
        databak = data;
    }

    vector<int> reset() {
        return data = databak;
    }

    vector<int> shuffle() {
        vector<int> rst;
        int n = data.size();
        int r;
        for (int i = 0; i < n; i++)
        {
            r = rand() % data.size();
            rst.push_back(data[r]);
            data.erase(data.begin() + r);
        }
        data = rst;
        return data;
    }
};

/**
 * Your Solution object will be instantiated and called as such:
 * Solution* obj = new Solution(nums);
 * vector<int> param_1 = obj->reset();
 * vector<int> param_2 = obj->shuffle();
 */