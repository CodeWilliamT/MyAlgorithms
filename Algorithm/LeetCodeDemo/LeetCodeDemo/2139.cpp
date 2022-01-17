using namespace std;
//巧思 从结果出发 模拟
class Solution {
public:
    int minMoves(int target, int maxDoubles) {
        int rst=0;
        int num = target,k=0;
        while (num>1) {
            if (k >= maxDoubles) {
                rst += num - 1;
                break;
            }
            if (num % 2) {
                num--;
            }
            else{
                num /= 2;
                k++;
            }
            rst++;
        }
        return rst;
    }
};