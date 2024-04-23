using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTree : MonoBehaviour
{
    public int life;
    public bool los;
    ITreeNode _root;
    private void Awake()
    {
        InitializeTree();
    }
    
    void InitializeTree()
    {
        //Actions
        ITreeNode dead = new ActionNode(() => print("Dead"));
        ITreeNode patrol = new ActionNode(() => print("Patrol"));
        ITreeNode attack = new ActionNode(() => print("Attack"));

        //Questions
        ITreeNode qAttack = new QuestionNode(QuestionAttack, attack, reload);
        ITreeNode qPratrol = new QuestionNode(QuestionBullet, patrol, reload);
        ITreeNode qLoS = new QuestionNode(QuestionLoS, qShoot, qPratrol);
        ITreeNode qHasLife = new QuestionNode(() => life > 0, qLoS, dead);

        _root = qHasLife;
    }

    public void ChangeTree(ITreeNode newTree)
    {
        _root = newTree;
    }

    public bool QuestionAttack()
    {
        return bullets > 0;
    }
    public bool QuestionLoS()
    {
        return los;
    }
}
