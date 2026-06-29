using UnityEngine;


public class ThirdPersonCameraController : MonoBehaviour
{
    public static ThirdPersonCameraController Instance { get; private set; }

    // ── Target ────────────────────────────────────────────────────────────────

    [Header("Target")]
    [Tooltip("Transform-ul personajului în jurul căruia orbitează camera.")]
    [SerializeField] private Transform target;

    [Tooltip("Pivot față de originea jucătorului (înălțimea umărului).")]
    [SerializeField] private Vector3 pivotOffset = new Vector3(0f, 1.5f, 0f);

    // ── Distance ──────────────────────────────────────────────────────────────

    [Header("Distance")]
    [SerializeField] private float defaultDistance = 4f;
    [SerializeField] private float minDistance     = 0.5f;

    // ── Sensitivity ───────────────────────────────────────────────────────────

    [Header("Sensitivity")]
    [SerializeField] private float sensitivityH = 2f;
    [SerializeField] private float sensitivityV = 1.5f;

    // ── Vertical Clamp ────────────────────────────────────────────────────────

    [Header("Vertical Angle")]
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch =  60f;

    // ── Smoothing ─────────────────────────────────────────────────────────────

    [Header("Smoothing")]
    [Tooltip("Cât de repede urmăresc unghiurile curente unghiurile țintă. " +
             "Valori mari = răspuns rapid, valori mici = mișcare mai flotantă.")]
    [SerializeField] private float rotationSmoothing = 15f;

    [Tooltip("Cât de repede se adaptează distanța la coliziuni (evită 'pop'-ul).")]
    [SerializeField] private float distanceSmoothing = 12f;

    // ── Collision ─────────────────────────────────────────────────────────────

    [Header("Collision")]
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private float collisionRadius = 0.25f;

    // ── Runtime State ─────────────────────────────────────────────────────────

    // Valori ȚINTĂ — actualizate direct din input
    private float _targetYaw;
    private float _targetPitch;

    // Valori CURENTE — urmăresc ținta smooth (folosite efectiv pentru poziție/rotație)
    private float _currentYaw;
    private float _currentPitch;
    private float _currentDistance;

    /// <summary>Yaw-ul curent (smooth) — util pentru rotația corpului jucătorului.</summary>
    public float Yaw => _currentYaw;

    // ── Unity Lifecycle ───────────────────────────────────────────────────────

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("[ThirdPersonCamera] Câmpul 'Target' nu este asignat!");
            enabled = false;
            return;
        }

        // Inițializăm din rotația curentă a camerei → fără salt brusc la Start
        _targetYaw    = _currentYaw    = transform.eulerAngles.y;
        _targetPitch  = _currentPitch  = transform.eulerAngles.x;
        _currentDistance = defaultDistance;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 1. Smooth pe unghiuri — LerpAngle gestionează corect wrap-ul la 360°
        _currentYaw   = Mathf.LerpAngle(_currentYaw,   _targetYaw,   rotationSmoothing * Time.deltaTime);
        _currentPitch = Mathf.LerpAngle(_currentPitch, _targetPitch, rotationSmoothing * Time.deltaTime);

        // 2. Pivot și rotație din unghiurile smooth
        Vector3    pivotWorld = target.position + pivotOffset;
        Quaternion rotation   = Quaternion.Euler(_currentPitch, _currentYaw, 0f);

        // Direcția de la pivot spre cameră (înapoi față de forward-ul camerei)
        Vector3 dirFromPivot = -(rotation * Vector3.forward);

        // 3. Collision: SphereCast de la pivot spre cameră
        float desiredDistance = defaultDistance;

        if (Physics.SphereCast(
                pivotWorld,
                collisionRadius,
                dirFromPivot,
                out RaycastHit hit,
                defaultDistance,
                collisionLayers,
                QueryTriggerInteraction.Ignore))
        {
            desiredDistance = Mathf.Max(hit.distance - collisionRadius, minDistance);
        }

        // 4. Smooth pe distanță (evită "pop"-ul brusc la intrare/ieșire din coliziune)
        _currentDistance = Mathf.Lerp(_currentDistance, desiredDistance, distanceSmoothing * Time.deltaTime);

        // 5. Aplicare — poziție calculată direct din unghiuri smooth → nicio desincronizare
        transform.position = pivotWorld + dirFromPivot * _currentDistance;
        transform.rotation = rotation;
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>
    /// Primește delta-ul de look de la PlayerController.ExecuteLook().
    /// Actualizează doar valorile ȚINTĂ; smoothing-ul se aplică în LateUpdate.
    /// </summary>
    public void AddLookInput(Vector2 lookDelta)
    {
        _targetYaw   += lookDelta.x * sensitivityH;
        _targetPitch  = Mathf.Clamp(
            _targetPitch - lookDelta.y * sensitivityV,
            minPitch, maxPitch);
    }
}