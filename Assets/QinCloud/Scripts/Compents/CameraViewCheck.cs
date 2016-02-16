using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 该类处理是否在摄像机内
/// </summary>

public class CameraViewCheck : MonoBehaviour 
{
    public Transform _target;
    public Camera _camera;
    private bool _gameObjectView = true;
    private float _targetSize_width; 
    private float _targetSize_height;
    private float _targetSize_depth; 
    private Vector3 _vector01;
    private Vector3 _vector02;
    private Vector3 _vector03;
    private Vector3 _vector04;
    private Vector3 _vector05;
    private Vector3 _vector06;
    private Vector3 _vector07;
    private Vector3 _vector08;
    private Vector3 _viewPos01;
    private Vector3 _viewPos02;
    private Vector3 _viewPos03;
    private Vector3 _viewPos04;
    private Vector3 _viewPos05;
    private Vector3 _viewPos06;
    private Vector3 _viewPos07;
    private Vector3 _viewPos08;
	// Use this for initialization
    public bool isVisiable;
    public List<string> blockNames = new List<string>();

	void Start () 
    {
        _target = this.transform;
        _camera = Camera.main;
        if (_target.GetComponent<Renderer>() != null)
        {
            Renderer render = _target.GetComponent<Renderer>();
            _targetSize_width = render.bounds.size.x;
            _targetSize_height = render.bounds.size.y;
            _targetSize_depth = render.bounds.size.z;
        }
  
	}
	
	// Update is called once per frame
	void Update ()
    {
        _vector01 = new Vector3(_target.position.x - _targetSize_width * 0.5f, _target.position.y + _targetSize_height * 0.5f, _target.position.z - _targetSize_depth * 0.5f);//摄像机视口左上角点1。
        _vector02 = new Vector3(_target.position.x + _targetSize_width * 0.5f, _target.position.y + _targetSize_height * 0.5f, _target.position.z - _targetSize_depth * 0.5f);//摄像机视口右上角点1。
        _vector03 = new Vector3(_target.position.x - _targetSize_width * 0.5f, _target.position.y - _targetSize_height * 0.5f, _target.position.z - _targetSize_depth * 0.5f);//摄像机视口左下角点1。
        _vector04 = new Vector3(_target.position.x + _targetSize_width * 0.5f, _target.position.y - _targetSize_height * 0.5f, _target.position.z - _targetSize_depth * 0.5f);//摄像机视口右下角点1。

        _vector05 = new Vector3(_target.position.x - _targetSize_width * 0.5f, _target.position.y + _targetSize_height * 0.5f, _target.position.z + _targetSize_depth * 0.5f);//摄像机视口左上角点2。
        _vector06 = new Vector3(_target.position.x + _targetSize_width * 0.5f, _target.position.y + _targetSize_height * 0.5f, _target.position.z + _targetSize_depth * 0.5f);//摄像机视口右上角点2。
        _vector07 = new Vector3(_target.position.x - _targetSize_width * 0.5f, _target.position.y - _targetSize_height * 0.5f, _target.position.z + _targetSize_depth * 0.5f);//摄像机视口左下角点2。
        _vector08 = new Vector3(_target.position.x + _targetSize_width * 0.5f, _target.position.y - _targetSize_height * 0.5f, _target.position.z + _targetSize_depth * 0.5f);//摄像机视口右下角点2。

        _viewPos01 = _camera.WorldToViewportPoint(_vector01);
        _viewPos02 = _camera.WorldToViewportPoint(_vector02);
        _viewPos03 = _camera.WorldToViewportPoint(_vector03);
        _viewPos04 = _camera.WorldToViewportPoint(_vector04);
        _viewPos05 = _camera.WorldToViewportPoint(_vector05);
        _viewPos06 = _camera.WorldToViewportPoint(_vector06);
        _viewPos07 = _camera.WorldToViewportPoint(_vector07);
        _viewPos08 = _camera.WorldToViewportPoint(_vector08);


        if (
            (_viewPos01.x > 1 || _viewPos01.x < 0 || _viewPos01.y > 1 || _viewPos01.y < 0) &&
            (_viewPos02.x > 1 || _viewPos02.x < 0 || _viewPos02.y > 1 || _viewPos02.y < 0) &&
            (_viewPos03.x > 1 || _viewPos03.x < 0 || _viewPos03.y > 1 || _viewPos03.y < 0) &&
            (_viewPos04.x > 1 || _viewPos04.x < 0 || _viewPos04.y > 1 || _viewPos04.y < 0) &&
            (_viewPos05.x > 1 || _viewPos05.x < 0 || _viewPos05.y > 1 || _viewPos05.y < 0) &&
            (_viewPos06.x > 1 || _viewPos06.x < 0 || _viewPos06.y > 1 || _viewPos06.y < 0) &&
            (_viewPos07.x > 1 || _viewPos07.x < 0 || _viewPos07.y > 1 || _viewPos07.y < 0) &&
            (_viewPos08.x > 1 || _viewPos08.x < 0 || _viewPos08.y > 1 || _viewPos08.y < 0)
            )
        {
            _gameObjectView = false;
        }
        else if
            (
            _viewPos01.z < 0 &&
            _viewPos02.z < 0 &&
            _viewPos03.z < 0 &&
            _viewPos04.z < 0 &&
            _viewPos05.z < 0 &&
            _viewPos06.z < 0 &&
            _viewPos07.z < 0 &&
            _viewPos08.z < 0
            )
        {
            _gameObjectView = false;
        }
        else
        {
            _gameObjectView = true;

        }

        Ray cameraToTarget = new Ray(Camera.main.transform.position, _target.position - Camera.main.transform.position);
        Debug.DrawLine(cameraToTarget.origin, cameraToTarget.direction * 100, Color.red);
        RaycastHit hit;
        Physics.Raycast(cameraToTarget, out hit);

        for(int i = 0 ; i < blockNames.Count; i++)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(blockNames[i]))
            {
                _gameObjectView = false;
                break;
            }
        }

        isVisiable = _gameObjectView;
	}

    void BoundsDrawRay()
    {
        Debug.DrawRay(_vector01, _vector02 - _vector01, Color.red);
        Debug.DrawRay(_vector02, _vector04 - _vector02, Color.red);
        Debug.DrawRay(_vector03, _vector01 - _vector03, Color.red);
        Debug.DrawRay(_vector04, _vector03 - _vector04, Color.red);
        
        Debug.DrawRay(_vector05, _vector06 - _vector05, Color.red);
        Debug.DrawRay(_vector06, _vector08 - _vector06, Color.red);
        Debug.DrawRay(_vector07, _vector05 - _vector07, Color.red);
        Debug.DrawRay(_vector08, _vector07 - _vector08, Color.red);
        
        Debug.DrawRay(_vector01, _vector05 - _vector01, Color.red);
        Debug.DrawRay(_vector02, _vector06 - _vector02, Color.red);
        Debug.DrawRay(_vector03, _vector07 - _vector03, Color.red);
        Debug.DrawRay(_vector04, _vector08 - _vector04, Color.red);
    }
}
