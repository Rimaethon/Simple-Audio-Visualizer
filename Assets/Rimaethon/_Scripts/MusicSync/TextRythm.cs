using System.Collections;
using System.Collections.Generic;
using Rimaethon._Scripts.Core.Interfaces;
using TMPro;
using UnityEngine;

namespace Rimaethon._Scripts.MusicSync
{

    public class TextRythm : MonoBehaviour
    {
        #region Fields
        private IAudioSpectrumProvider _audioSpectrumProvider;
        private TMP_Text _textComponent;
        private bool _hasTextChanged;
        

        #endregion



        #region MonoBehaviour Functions
        void Awake()
        {
            _textComponent = GetComponent<TMP_Text>();
            _audioSpectrumProvider = GetComponentInParent<IAudioSpectrumProvider>();
            if (_audioSpectrumProvider==null)
            {
                Debug.Log("Audio Spectrum provider Is Null ");

            }
        }

        void OnEnable()
        {
            // Subscribe to event fired when text object has been regenerated.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
        }

        void OnDisable()
        {
            // UnSubscribe to event fired when text object has been regenerated.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
        }


        void Start()
        {
            StartCoroutine(AnimateVertexColors());
        }

        

        #endregion


        #region Private Functions
        private void ON_TEXT_CHANGED(Object obj)
        {
            if (obj == _textComponent)
                _hasTextChanged = true;
        }
        

        #endregion




        #region Enumerators

        IEnumerator AnimateVertexColors()
        {


            _textComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = _textComponent.textInfo;

            Matrix4x4 matrix;
            TMP_MeshInfo[] cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();

            // Allocations for sorting of the modified scales
            List<float> modifiedCharScale = new List<float>();
            List<int> scaleSortingOrder = new List<int>();

            _hasTextChanged = true;

            while (true)
            {
                if (_hasTextChanged)
                {
                    cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();

                    _hasTextChanged = false;
                }

                int characterCount = textInfo.characterCount;

                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }

                // Clear list of character scales
                modifiedCharScale.Clear();
                scaleSortingOrder.Clear();

                for (int i = 0; i < characterCount; i++)
                {
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                    if (!charInfo.isVisible)
                        continue;

                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    Vector3[] sourceVertices = cachedMeshInfoVertexData[materialIndex].vertices;

                    Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

                    // Need to translate all 4 vertices of each quad to aligned with middle of character / baseline.
                    // This is needed so the matrix TRS is applied at the origin for each character.
                    Vector3 offset = charMidBasline;

                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                    destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                    destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                    destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                    destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

                    float peakLevel = _audioSpectrumProvider.GetPeakLevel(0);
                    float randomScale = Random.Range(1f, peakLevel * 10);

                    modifiedCharScale.Add(randomScale);
                    scaleSortingOrder.Add(modifiedCharScale.Count - 1);


                    matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, Vector3.one * randomScale);

                    destinationVertices[vertexIndex + 0] =
                        matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
                    destinationVertices[vertexIndex + 1] =
                        matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
                    destinationVertices[vertexIndex + 2] =
                        matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
                    destinationVertices[vertexIndex + 3] =
                        matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

                    destinationVertices[vertexIndex + 0] += offset;
                    destinationVertices[vertexIndex + 1] += offset;
                    destinationVertices[vertexIndex + 2] += offset;
                    destinationVertices[vertexIndex + 3] += offset;

                    Vector2[] sourceUVs0 = cachedMeshInfoVertexData[materialIndex].uvs0;
                    Vector2[] destinationUVs0 = textInfo.meshInfo[materialIndex].uvs0;

                    destinationUVs0[vertexIndex + 0] = sourceUVs0[vertexIndex + 0];
                    destinationUVs0[vertexIndex + 1] = sourceUVs0[vertexIndex + 1];
                    destinationUVs0[vertexIndex + 2] = sourceUVs0[vertexIndex + 2];
                    destinationUVs0[vertexIndex + 3] = sourceUVs0[vertexIndex + 3];

                    Color32[] sourceColors32 = cachedMeshInfoVertexData[materialIndex].colors32;
                    Color32[] destinationColors32 = textInfo.meshInfo[materialIndex].colors32;

                    destinationColors32[vertexIndex + 0] = sourceColors32[vertexIndex + 0];
                    destinationColors32[vertexIndex + 1] = sourceColors32[vertexIndex + 1];
                    destinationColors32[vertexIndex + 2] = sourceColors32[vertexIndex + 2];
                    destinationColors32[vertexIndex + 3] = sourceColors32[vertexIndex + 3];
                }

                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    scaleSortingOrder.Sort((a, b) => modifiedCharScale[a].CompareTo(modifiedCharScale[b]));

                    textInfo.meshInfo[i].SortGeometry(scaleSortingOrder);

                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    textInfo.meshInfo[i].mesh.uv = textInfo.meshInfo[i].uvs0;
                    textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;

                    _textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                yield return new WaitForSeconds(0.1f);
            }

            // ReSharper disable once IteratorNeverReturns
        }


        #endregion
      

    }
}