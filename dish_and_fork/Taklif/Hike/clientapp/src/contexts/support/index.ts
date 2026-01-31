import constate from 'constate';

import { useSupportContext } from './useSupportContext';

const [SupportProvider, useSupportState, useSupportActions] = constate(
  useSupportContext,
  ([state]) => state,
  ([, actions]) => actions,
);

export { SupportProvider, useSupportActions, useSupportState };
export * from './types';
