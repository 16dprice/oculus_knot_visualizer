using Domain;

namespace PDCodeTEMP
{
    public class PDCodeBead
    {
        public Bead bead;

        private PDCodeBead _next;
        private PDCodeBead _prev;

        public PDCodeBead(Bead bead, PDCodeBead prev, PDCodeBead next)
        {
            this.bead = bead;
            _prev = prev;
            _next = next;
        }

        public PDCodeBead GetPrev() => _prev;
        public PDCodeBead GetNext() => _next;

        public void SetPrev(PDCodeBead prevBead)
        {
            _prev = prevBead;
            prevBead._next = this;
        }

        public void SetNext(PDCodeBead nextBead)
        {
            _next = nextBead;
            nextBead._prev = this;
        }

        public void InsertAfter(PDCodeBead nextBead)
        {
            nextBead._next = _next;
            nextBead._prev = this;

            _next._prev = nextBead;
            _next = nextBead;
        }

        public bool IsBeadAdjacent(PDCodeBead other)
        {
            if (other == _next) return true;
            if (other == _prev) return true;

            return false;
        }
    }
}