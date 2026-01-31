import { useCallback } from 'react';
import { useSearchParams } from 'react-router-dom';

import { parseIntOrInit } from '~/utils';

type SetParam = (value: number) => void;
type UseParam = (paramName: string, initial: number) => [number, SetParam];

const useParam: UseParam = (paramName, initial) => {
  const [params, setParams] = useSearchParams();
  const param = parseIntOrInit(params.get(paramName), initial);

  const setParam = useCallback<SetParam>(
    (value) => {
      setParams({ ...Object.fromEntries(params.entries()), [paramName]: String(value) });
    },
    [paramName, params, setParams],
  );

  return [param, setParam];
};

export { useParam };
export type { SetParam, UseParam };
