using UnityEngine;

public class EnemySelector : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxSelectionDistance = 20f;
    [SerializeField] private LayerMask enemyLayer;

    private Transform selectedEnemy;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxSelectionDistance, enemyLayer))
        {
            selectedEnemy = hit.transform;
        }
        else
        {
            selectedEnemy = null;
        }

        if (selectedEnemy != null)
            Debug.Log("Apuntando a: " + selectedEnemy.name);
        else
            Debug.Log("Ningún enemigo seleccionado");


    }

    public Transform GetSelectedEnemy()
    {
        return selectedEnemy;
    }
}
