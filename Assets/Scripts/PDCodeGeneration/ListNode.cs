namespace PDCodeGeneration
{
    public class ListNode
    {
        public PDCodeBead bead;
        public ListNode prev;
        public ListNode next;
        
        public ListNode(PDCodeBead bead, ListNode prev, ListNode next)
        {
            this.bead = bead;
            this.prev = prev;
            this.next = next;
        }

        public void SetPrev(ListNode node)
        {
            prev = node;
            node.next = this;
        }

        public void SetNext(ListNode node)
        {
            next = node;
            node.prev = this;
        }
    }
}