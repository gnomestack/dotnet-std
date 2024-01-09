namespace Tests;

// ReSharper disable once InconsistentNaming
public class EnvExpand_Tests
{
    [UnitTest]
    public void EvaluateNothing(IAssert assert)
    {
        var result = Env.ExpandVars("Hello World");
        assert.Equal("Hello World", result);
    }

    [UnitTest]
    public void EvaluateEscapedName(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        var result = Env.ExpandVars("Hello \\$WORD");
        assert.Equal("Hello $WORD", result);

        result = Env.ExpandVars("Hello $WORD\\_SUN");
        assert.Equal("Hello World_SUN", result);
    }

    [UnitTest]
    public void EvaluateDoubleBashVar(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");
        Environment.SetEnvironmentVariable("HELLO", "Hello");

        var result = Env.ExpandVars("$HELLO $WORD");
        assert.Equal("Hello World", result);

        result = Env.ExpandVars("$HELLO$WORD!");
        assert.Equal("HelloWorld!", result);
    }

    [UnitTest]
    public void EvaluateSingleWindowsVar(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        var result = Env.ExpandVars("Hello %WORD%");
        assert.Equal("Hello World", result);

        result = Env.ExpandVars("Hello test%WORD%:");
        assert.Equal("Hello testWorld:", result);

        result = Env.ExpandVars("%WORD%");
        assert.Equal("World", result);

        result = Env.ExpandVars("%WORD%  ");
        assert.Equal("World  ", result);

        result = Env.ExpandVars(" \n%WORD%  ");
        assert.Equal(" \nWorld  ", result);
    }

    [UnitTest]
    public void EvaluateSingleBashVar(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        var result = Env.ExpandVars("Hello $WORD");
        assert.Equal("Hello World", result);

        result = Env.ExpandVars("Hello test$WORD:");
        assert.Equal("Hello testWorld:", result);

        result = Env.ExpandVars("$WORD");
        assert.Equal("World", result);

        result = Env.ExpandVars("$WORD  ");
        assert.Equal("World  ", result);

        result = Env.ExpandVars(" \n$WORD  ");
        assert.Equal(" \nWorld  ", result);
    }

    [UnitTest]
    public void EvaluateSingleInterpolatedBashVar(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        var result = Env.ExpandVars("Hello ${WORD}");
        assert.Equal("Hello World", result);

        result = Env.ExpandVars("Hello test${WORD}:");
        assert.Equal("Hello testWorld:", result);

        result = Env.ExpandVars("${WORD}");
        assert.Equal("World", result);

        result = Env.ExpandVars("${WORD}  ");
        assert.Equal("World  ", result);

        result = Env.ExpandVars(" \n$WORD  ");
        assert.Equal(" \nWorld  ", result);
    }

    [UnitTest]
    public void UseDefaultValueForBashVar(IAssert assert)
    {
        // assert state
        assert.False(Env.Vars.Contains("WORD2"));

        var result = Env.ExpandVars("${WORD2:-World}");
        assert.Equal("World", result);
        assert.False(Env.Vars.Contains("WORD2"));
    }

    [UnitTest]
    public void SetEnvValueWithBashVarWhenNull(IAssert assert)
    {
        // assert state
        assert.False(Env.Vars.Contains("WORD3"));

        var result = Env.ExpandVars("${WORD3:=World}");
        assert.Equal("World", result);
        assert.True(Env.Vars.Contains("WORD3"));
        assert.Equal("World", Env.Var("WORD3"));
    }

    [UnitTest]
    public void ThrowOnMissingBashVar(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        var ex = assert.Throws<EnvExpandException>(() =>
        {
            Env.ExpandVars("Hello ${WORLD:?WORLD must be set}");
        });

        assert.Equal("WORLD must be set", ex.Message);

        ex = assert.Throws<EnvExpandException>(() =>
        {
            Env.ExpandVars("Hello ${WORLD}");
        });

        assert.Equal("Bad substitution, variable WORLD is not set.", ex.Message);

        ex = assert.Throws<EnvExpandException>(() =>
        {
            Env.ExpandVars("Hello $WORLD");
        });

        assert.Equal("Bad substitution, variable WORLD is not set.", ex.Message);
    }

    [UnitTest]
    public void UnclosedToken_Exception(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        assert.Throws<EnvExpandException>(() =>
        {
            Env.ExpandVars("Hello ${WORD");
        });

        assert.Throws<EnvExpandException>(() =>
        {
            Env.ExpandVars("Hello %WORD");
        });
    }
}