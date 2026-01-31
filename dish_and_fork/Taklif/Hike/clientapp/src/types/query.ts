import { AppErrorModel, AppProblemDetails, ErrorModel } from './swagger';

export type UseQueryError = AppProblemDetails | AppErrorModel | ErrorModel | undefined;
