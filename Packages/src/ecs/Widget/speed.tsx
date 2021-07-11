import { Item } from '@/Widget/Item';

type TState = {
    test: boolean, speedValue: number
}

export class speed extends Item {
    state: TState
    
    constructor(props) {
        super(props);
        this.state = {
            speedValue: this.speedValue,
            test: true,
        }
    }
    
    public speedValue: number = 0;
    
    public render(): React.ReactNode {
        this.setState({ speedValue: this.speedValue });
        return super.render();
    }
}