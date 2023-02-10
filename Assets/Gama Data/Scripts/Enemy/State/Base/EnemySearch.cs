using UnityEngine;
using UnityEngine.AI;

public class EnemySearch : BaseEnemyState
{
    public EnemySearch(BaseEnemyController enemyController, BaseEnemyStateController enemyStateController) : base(enemyController, enemyStateController) { }

    public override void Update()
    {
        p_Controller.EnemySearcher.Search();

        base.Update();
    }

    public override void TransitionLogic()
    {
        base.TransitionLogic();

        if (p_Controller.IsFollow == false)
        {
            p_StateController.SwichState(p_ThisState);
        }
    }
}
