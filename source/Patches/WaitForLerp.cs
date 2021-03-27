using System;
using System.Collections;
using UnityEngine;

namespace TownOfUs
{
    public class WaitForLerp : IEnumerator
    {
        // Token: 0x06000139 RID: 313 RVA: 0x00007A6E File Offset: 0x00005C6E
        public WaitForLerp(float seconds, Action<float> act)
        {
            this.duration = seconds;
            this.act = act;
        }

        // Token: 0x17000019 RID: 25
        // (get) Token: 0x0600013A RID: 314 RVA: 0x00007A84 File Offset: 0x00005C84
        public object Current
        {
            get
            {
                return null;
            }
        }

        // Token: 0x0600013B RID: 315 RVA: 0x00007A88 File Offset: 0x00005C88
        public bool MoveNext()
        {
            this.timer = Mathf.Min(this.timer + Time.deltaTime, this.duration);
            this.act(this.timer / this.duration);
            return this.timer < this.duration;
        }

        // Token: 0x0600013C RID: 316 RVA: 0x00007AD8 File Offset: 0x00005CD8
        public void Reset()
        {
            this.timer = 0f;
        }

        // Token: 0x04000117 RID: 279
        private float duration;

        // Token: 0x04000118 RID: 280
        private float timer;

        // Token: 0x04000119 RID: 281
        private Action<float> act;
    }
}