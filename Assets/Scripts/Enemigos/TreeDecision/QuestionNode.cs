using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : ITreeNode
{
    public Func<bool> _question;
    ITreeNode _fNode;
    ITreeNode _tNode;
    ITreeNode _trNode;
    public QuestionNode(Func<bool> question, ITreeNode tNode, ITreeNode fNode)
    {
        _question = question;
        _tNode = tNode;
        _fNode = fNode;
    }
    public QuestionNode(Func<bool> question, ITreeNode tNodePatrulla)
    {
        _question = question;
        _trNode = tNodePatrulla;

    }
    public void Execute()
    {
        if (_question()) _tNode.Execute();
        else _fNode.Execute();
    }
}
