using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HouseGeneration : MonoBehaviour
{
   [SerializeField] private GameObject _topPos;
   [SerializeField] private GameObject _bottomPos;

   [SerializeField] private SpriteRenderer _partTemplate;
   
   [SerializeField] private Sprite[] _possibleTopParts;
   [FormerlySerializedAs("_possibleParts")]
   [SerializeField] private Sprite[] _possibleBottomParts;
   
   private Sprite[] _houseTopParts;
   private Sprite[] _houseBotParts;
   public int test = 0;
   void Start()
   {
      int i = test * 2;
      _topPos.transform.localPosition = new Vector2(0, 6.5f);
     InitializeBottom(i);
     InitializeTop(i);
   }

   private void InitializeBottom(int i)
   {
      _houseBotParts = new Sprite[i + 3];
      _houseBotParts[0] = _possibleBottomParts[0];
      _houseBotParts[i/2 + 1] = _possibleBottomParts[2];
      _houseBotParts[i + 2] = _possibleBottomParts[4];

      for (int j = 0; j < i / 2; j++)
      {
         _houseBotParts[j + 1] = _possibleBottomParts[1];
         _houseBotParts[i + 1 - j] = _possibleBottomParts[3];
      }
      Vector2 relativePos = new Vector2(0, 0);
      var piece = Instantiate(_partTemplate, _bottomPos.transform);
      piece.sprite = _houseBotParts[0];
      piece.gameObject.transform.localPosition = new Vector2(relativePos.x, piece.gameObject.transform.localPosition.y);
      relativePos.x += piece.sprite.bounds.size.x/2;

      for (int k = 1; k < _houseBotParts.Length; k++)
      {
         piece = Instantiate(_partTemplate, _bottomPos.transform);
         piece.sprite = _houseBotParts[k];
         var tempX = piece.sprite.bounds.size.x ;
         relativePos.x += tempX/2;

         piece.gameObject.transform.localPosition = new Vector2(relativePos.x, piece.gameObject.transform.localPosition.y);
         relativePos.x += tempX/2;
      }
   }
   private void InitializeTop(int i)
   {
      _houseTopParts = new Sprite[i + 3];
      _houseTopParts[0] = _possibleTopParts[0];
      _houseTopParts[i/2 + 1] = _possibleTopParts[2];
      _houseTopParts[i + 2] = _possibleTopParts[4];

      for (int j = 0; j < i / 2; j++)
      {
         _houseTopParts[j + 1] = _possibleTopParts[1];
         _houseTopParts[i + 1 - j] = _possibleTopParts[3];
      }
      Vector2 relativePos = new Vector2(0, 0);
      var piece = Instantiate(_partTemplate, _topPos.transform);
      piece.sprite = _houseTopParts[0];
      piece.gameObject.transform.localPosition = new Vector2(relativePos.x, piece.gameObject.transform.localPosition.y);
      relativePos.x += piece.sprite.bounds.size.x/2;

      for (int k = 1; k < _houseTopParts.Length; k++)
      {
         piece = Instantiate(_partTemplate, _topPos.transform);
         piece.sprite = _houseTopParts[k];
         var tempX = piece.sprite.bounds.size.x ;
         relativePos.x += tempX/2;

         piece.gameObject.transform.localPosition = new Vector2(relativePos.x, piece.gameObject.transform.localPosition.y);
         relativePos.x += tempX/2;
      }
   }
}
