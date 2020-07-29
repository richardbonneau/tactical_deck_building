using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    public UiManager uiManager;
    public GridCreator gridCreator;
    public GameObject player;
    Animator playerAnimator;
    [System.NonSerialized] public CardAbilities originCard;
    public EnemiesManager enemiesManager;
    bool hasToChooseMeleetarget = false;
    bool hasToChooseAoeTarget = false;
    List<GameObject> targets = new List<GameObject>();
    public Camera mainCam;
    public GameObject mapSelector;
    public GameObject potentialTargetIndicator;
    List<GameObject> targetIndicators = new List<GameObject>();
    int attackAmount = 0;
    public ObjectPooler attackIndicatorsPool;
    List<GameObject> attackIndicators = new List<GameObject>();
    public GameObject aoeExplosionEffect;
    public GameObject castingAoe;
    AudioSource audioSource;

    public AudioClip sword;
    public AudioClip area;


    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
    }
    void Update()
    {
        if (hasToChooseMeleetarget) ChoosingMeleeTarget();
        else if (hasToChooseAoeTarget) ChoosingAoeTarget();
    }

    void ChoosingMeleeTarget()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mouseSelectWorldPosition = new Vector3(Mathf.Round(hit.point.x), 0, Mathf.Round(hit.point.z));
            Transform objectHit = hit.transform;
            mapSelector.transform.position = mouseSelectWorldPosition;
            GameObject target = targets.Find(enemy => enemy.transform.position == mouseSelectWorldPosition);
            if (Input.GetMouseButtonDown(0) && target != null)
            {
                AttackTarget(target);
            }
        }
    }
    void ChoosingAoeTarget()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mouseSelectWorldPosition = new Vector3(Mathf.Round(hit.point.x), 0, Mathf.Round(hit.point.z));

            if (attackIndicators.Count > 0)
            {
                playerAnimator.SetBool("area", true);
                castingAoe.SetActive(true);
                player.transform.LookAt(attackIndicators[attackIndicators.Count - 1].transform);
                // AOE AREA CHOSEN
                if (Input.GetMouseButtonDown(0))
                {
                    hasToChooseAoeTarget = false;
                    print("AOE ATTACK");
                    aoeExplosionEffect.transform.position = attackIndicators[attackIndicators.Count - 1].transform.position;
                    StartCoroutine(AoeExplosion());
                    List<GameObject> enemies = enemiesManager.activeEnemies;
                    List<GameObject> targets = new List<GameObject>();
                    foreach (GameObject indicator in attackIndicators)
                    {
                        GameObject enemyInZone = enemies.Find(enemy => enemy.transform.position == indicator.transform.position);
                        if (enemyInZone != null) targets.Add(enemyInZone);
                    }

                    foreach (GameObject indicator in attackIndicators) indicator.SetActive(false);
                    attackIndicators.Clear();


                    foreach (GameObject enemy in targets)
                    {
                        enemy.GetComponent<EnemyStatus>().GetHit(attackAmount);
                    }
                    playerAnimator.SetBool("area", false);
                    castingAoe.SetActive(false);
                    StartCoroutine(EndOfAttackDelay());
                    return;
                }
                // END OF AOE AREA CHOSEN

                if (attackIndicators[attackIndicators.Count - 1].transform.position == mouseSelectWorldPosition) return;
                foreach (GameObject indicator in attackIndicators)
                {
                    indicator.SetActive(false);
                }
                attackIndicators.Clear();
            }

            Transform objectHit = hit.transform;
            List<Node> mousePositionNeighbours = gridCreator.GetNeighbours(gridCreator.NodeFromWorldPoint(mouseSelectWorldPosition));
            mousePositionNeighbours.Add(gridCreator.NodeFromWorldPoint(mouseSelectWorldPosition));
            foreach (Node node in mousePositionNeighbours)
            {
                GameObject p = attackIndicatorsPool.GetPooledObject();
                if (p != null)
                {
                    p.transform.position = node.worldPosition;
                    p.SetActive(true);
                }
                attackIndicators.Add(p);
            }
        }
    }

    private IEnumerator AoeExplosion()
    {
        audioSource.clip = area;
        audioSource.Play();
        aoeExplosionEffect.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        aoeExplosionEffect.SetActive(false);
    }
    private IEnumerator EndOfAttackDelay()
    {
        yield return new WaitForSeconds(1.5f);
        originCard.NextAction();
    }

    public void AreaAttack(int atkAmount)
    {
        attackAmount = atkAmount;
        hasToChooseAoeTarget = true;
    }
    public void FindPotentialTargets(int atkAmount)
    {
        attackAmount = atkAmount;
        List<GameObject> enemies = enemiesManager.activeEnemies;
        List<Node> neighboursNodes = gridCreator.GetNeighbours(gridCreator.NodeFromWorldPoint(player.transform.position));
        foreach (Node node in neighboursNodes)
        {
            GameObject found = enemies.Find(enemy => enemy.transform.position.x == node.worldPosition.x && enemy.transform.position.z == node.worldPosition.z);
            if (found != null)
            {
                GameObject potentialTarget = Instantiate(potentialTargetIndicator, found.transform.position, Quaternion.identity);
                targetIndicators.Add(potentialTarget);
                targets.Add(found);
                hasToChooseMeleetarget = true;
            }
        }
        if (targets.Count == 0)
        {
            StartCoroutine(NoTargetsToAttack());
        }
    }
    private IEnumerator NoTargetsToAttack()
    {
        uiManager.DisplayNoTargetsMessage("Your Attack Ability has no targets.");
        yield return new WaitForSeconds(2f);
        originCard.NextAction();
    }
    void AttackTarget(GameObject target)
    {
        foreach (GameObject indicator in targetIndicators) Destroy(indicator);
        targetIndicators.Clear();
        targets.Clear();
        hasToChooseMeleetarget = false;
        playerAnimator.SetTrigger("Attack");
        player.transform.LookAt(target.transform.position);

        target.GetComponent<EnemyStatus>().GetHit(attackAmount);
        StartCoroutine(EndOfAttackDelay());
        audioSource.clip = sword;
        audioSource.Play();
    }


}
