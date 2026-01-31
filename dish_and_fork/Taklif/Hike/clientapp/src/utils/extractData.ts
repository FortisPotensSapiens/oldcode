import { AxiosResponse } from 'axios';

type ExtractData = <R1, R2>(promise: Promise<AxiosResponse<R1, R2>>) => Promise<R1>;

const extractData: ExtractData = (promise) => promise.then(({ data }) => data);

export { extractData };
