using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RatCow.MvcFramework.Mapping
{
  //this doesn't really need to be public, but ISTR that if it isn't, .Net won't let us use it publicly
  //and therefore it breaks the other interfaces..
  public interface IBaseMappingObject
  {
    void Pull();

    void Pull(bool binding);

    void Revert();

    void Subscribe(Control aSubject);

    void Subscribe(Control aSubject, ListControlMapping aMapping); //ComboBox and othe ListControl derrived controls

    bool Modified { get; }

    void Snapshot();

    bool IsMultiValueItem { get; set; }

    Control LinkedControl { get; set; }

    event DataModificationDelegate ValueModificationQuery;
    event System.EventHandler ValueWasModified;
    event ValidationErrorDelegate ValidationError;
    event BeforeDataModificationDelegate BeforeValueModified;
  }

  //This is a "nice to have" interface
  public interface IMappingObject<T> : IBaseMappingObject
  {
    T CurrentValue { get; set; }

    T OriginalValue { get; }

    void Init(T aValue);

    void CustomizedSubscribe(MappingUpdateSubscriber<T> aSubscription);
  }

  //this one we actually use. It has all the basics we need to interact with a generic class instance we
  //dynamically create... wonders of generics!
  public interface IMappingObject : IBaseMappingObject
  {
    object CurrentObject { get; set; }

    object OriginalObject { get; }

    void InitWithObject(object data, string propertyName);
  }
}