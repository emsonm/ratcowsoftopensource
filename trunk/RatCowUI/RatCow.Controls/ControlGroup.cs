/*
 * Copyright 2013 Rat Cow Software and Matt Emson. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this list of
 *    conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list
 *    of conditions and the following disclaimer in the documentation and/or other materials
 *    provided with the distribution.
 * 3. Neither the name of the Rat Cow Software nor the names of its contributors may be used
 *    to endorse or promote products derived from this software without specific prior written
 *    permission.
 *
 * THIS SOFTWARE IS PROVIDED BY RAT COW SOFTWARE "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied, of Rat Cow Software and Matt Emson.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Controls
{
    public class ControlGroup
    {
        public ControlGroup()
        {
            Items = new List<IGroupControl>();
        }

        public List<IGroupControl> Items { get; protected set; }

        public void Add(IGroupControl item)
        {
            item.Owner = this;
            item.NotifyMembersAction += new NotifyMembersActionDelegate(item_NotifyMembersAction);
            item.NotifyMembersGeneral += new NotifyMembersGeneralDelegate(item_NotifyMembersGeneral);
            item.NotifyMembersState += new NotifyMembersStateDelegate(item_NotifyMembersState);

            Items.Add(item);
        }

        void item_NotifyMembersState(IGroupControl sender, bool state)
        {
            foreach (var item in Items)
            {
                if (item != sender)
                {
                    item.Notified(sender, new GroupNotificationArgs() { State = state, Data = null });
                }
            }
            
        }

        void item_NotifyMembersGeneral(IGroupControl sender)
        {
            foreach (var item in Items)
            {
                if (item != sender)
                {
                    item.Notified(sender, new GroupNotificationArgs() { State = false, Data = null });
                }
            }
        }

        void item_NotifyMembersAction(IGroupControl sender, GroupNotificationArgs e)
        {
            foreach (var item in Items)
            {
                if (item != sender)
                {
                    item.Notified(sender, e);
                }
            }
        }

        
    }
}
