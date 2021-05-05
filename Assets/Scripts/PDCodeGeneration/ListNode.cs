namespace PDCodeGeneration
{
    public class ListNode
    {
        private PDCodeBead _bead;
        private ListNode _prev;
        private ListNode _next;
        
        public ListNode(PDCodeBead bead, ListNode prev, ListNode next)
        {
            _bead = bead;
            _prev = prev;
            _next = next;
        }

        public void SetPrev(ListNode node)
        {
            _prev = node;
            node._next = this;
        }

        public void SetNext(ListNode node)
        {
            _next = node;
            node._prev = this;
        }
    }
}