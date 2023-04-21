import { makeAutoObservable, runInAction } from "mobx";
import { Rubric } from "../models/rubric";
import agent from "../api/agent";

export default class RubricStore {
    rubrics: Rubric[] = [];
    selectedRubric: Rubric | undefined = undefined;
    loading = false;
    loadingIntial = false;
    rubricOptions: any[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    loadRubrics = async () => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.Rubrics.list(null);
            result.forEach(rubric => this.setRubric(rubric));
            runInAction(() => {
                this.rubrics.flatMap(rubric => rubric.rubrics ? [rubric, ...rubric.rubrics] : rubric)
                    .forEach(rubric => this.setRubricOption(rubric));
            })
            this.setLoadingInitial(false);
        } catch (error) {
            runInAction(() => {
                console.log(error);
            })
            this.setLoadingInitial(false);
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingIntial = state;
    }

    private setRubric = (rubric: Rubric) => {
        this.rubrics.push(rubric);
    }

    private setRubricOption = (rubric: Rubric) => {
        this.rubricOptions.push({ key: rubric.id, value: rubric.id, text: rubric.name });
    }
}