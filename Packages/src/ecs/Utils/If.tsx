const If = (props) => {
    const condition = props.condition || false;
    const positive = props.then || null;
    const negative = props.else || null;
    
    return condition ? positive : negative;
};

/*
<IF condition={isLoggedIn} then={<Hello />} else={<div>请先登录</div>} />
 https://segmentfault.com/a/1190000025135870
 React 条件渲染最佳实践(7 种方法)
 */
