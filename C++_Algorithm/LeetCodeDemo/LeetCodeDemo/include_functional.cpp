using namespace std;
#include <functional>
class Solution {
public:
    void include_functional() {
        function<bool(int, int)> check = [&](int p, int q) {
            return p < q;
        };
    }
}；