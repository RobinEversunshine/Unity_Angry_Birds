using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using DG.Tweening;

public class SlingShotHandler : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer _leftlineRenderer;
    [SerializeField] private LineRenderer _rightlineRenderer;

    [Header("Transform References")]
    [SerializeField] private Transform _leftStartPos;
    [SerializeField] private Transform _rightStartPos;
    [SerializeField] private Transform _centerPos;
    [SerializeField] private Transform _elasticXform;

    [Header("Slingshot Stats")]
    [SerializeField] private float _maxDist = 3.5f;
    [SerializeField] private float _shotForce = 5f;
    [SerializeField] private float _elasticDivider = 3f;
    [SerializeField] private AnimationCurve _elasticCurve;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea _area;
    [SerializeField] private CameraManager _cameraManager;


    [Header("Bird")]
    [SerializeField] private AngryBird _birdPrefab;
    [SerializeField] private float _posOffset = 0.3f;

    [Header("Sounds")]
    [SerializeField] private AudioClip _elasticPulledClip;
    [SerializeField] private AudioClip[] _elasticReleasedClip;






    private Vector2 _slingShotLinesPos;
    private Vector2 _dir;
    private Vector2 _dirNorm;

    private bool _clickedWithinArea;
    private bool _birdOnSlingShot;

    private AngryBird _spawnedBird;
    private AudioSource _audioSource;

    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        SpawnBird();
        SetLines(_centerPos.position);
    }

    
    void Update()
    {   
        if (Mouse.current.leftButton.wasPressedThisFrame && _area.IsWithinSlingShot())
        {
            _clickedWithinArea = true;

            if (_birdOnSlingShot)
            {
                SoundManager.instance.PlayClip(_elasticPulledClip, _audioSource);
                _cameraManager.SwitchToFollow(_spawnedBird.transform);
            }

            //if (_spawnedBird._hasLaunched)
            //{
            //    SpawnBird();
            //}
        }

        if (Mouse.current.leftButton.isPressed && _clickedWithinArea && _birdOnSlingShot)
        {
            DrawSlingShot();
            PositionBird();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _birdOnSlingShot && _clickedWithinArea)
        {
            
            _clickedWithinArea = false;
            //SetLines(_centerPos.position);
            AnimateSlingShot();
            _spawnedBird.launch(_dir, _shotForce);
            _birdOnSlingShot = false;

            SoundManager.instance.PlayRandomClip(_elasticReleasedClip, _audioSource);


            GameManager.instance.UseShot();

            if (GameManager.instance.HasEnoughShots())
            {
                StartCoroutine(SpawnBirdAfterTime());

            }

        }
    }

    #region slingShot Methods

    private void DrawSlingShot()
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        touchPos.z = 0;
        
        _slingShotLinesPos = _centerPos.position + Vector3.ClampMagnitude(touchPos - _centerPos.position, _maxDist);

        SetLines(_slingShotLinesPos);

        _dir = (Vector2)_centerPos.position - _slingShotLinesPos;
        _dirNorm = _dir.normalized;
    }

    private void SetLines(Vector2 pos)
    {
        _leftlineRenderer.SetPosition(0, pos);
        _leftlineRenderer.SetPosition(1, _leftStartPos.position);

        _rightlineRenderer.SetPosition(0, pos);
        _rightlineRenderer.SetPosition(1, _rightStartPos.position);
    }

    #endregion



    #region Bird Methods

    private void SpawnBird()
    {
        _spawnedBird = Instantiate(_birdPrefab, _centerPos.position, Quaternion.identity);
        _birdOnSlingShot = true;

    }

    private void PositionBird()
    {
        _spawnedBird.transform.position = _slingShotLinesPos + _dirNorm * _posOffset;
        _spawnedBird.transform.right = _dirNorm;
    }


    private IEnumerator SpawnBirdAfterTime()
    {
        yield return new WaitForSeconds(2f);
        SpawnBird();
        _cameraManager.SwitchToIdle();
    }





    #endregion


    #region Animate Sling Shot

    private void AnimateSlingShot()
    {
        _elasticXform.position = _leftlineRenderer.GetPosition(0);

        float dist = Vector2.Distance(_elasticXform.position, _centerPos.position);
        float time = dist / _elasticDivider;
        _elasticXform.DOMove(_centerPos.position, time).SetEase(_elasticCurve);

        StartCoroutine(AnimateSlingShotLines(_elasticXform, time));
    }

    private IEnumerator AnimateSlingShotLines(Transform xform, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            SetLines(xform.position);
            yield return null;
        }

    }

    #endregion

}
