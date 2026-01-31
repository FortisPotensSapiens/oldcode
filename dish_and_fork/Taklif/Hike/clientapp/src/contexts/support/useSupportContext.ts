import { useCallback, useState } from 'react';

import { SupportContext } from './types';

type UseSupportContext = (props?: { initial?: boolean }) => SupportContext;

const useSupportContext: UseSupportContext = (props) => {
  const [state, change] = useState(props?.initial ?? false);

  const hide = useCallback(() => change(false), []);
  const show = useCallback(() => change(true), []);

  return [state, { change, hide, show }];
};

export { useSupportContext };
export type { UseSupportContext };
