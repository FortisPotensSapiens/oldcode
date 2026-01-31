import { ComponentType, Suspense } from 'react';

import { LoadingSpinner } from '~/components';

const withSuspend = <P extends Record<string, unknown> = Record<string, never>>(
  Component: ComponentType<P>,
): ComponentType<P> => {
  const Suspended = (props: P) => (
    <Suspense fallback={<LoadingSpinner />}>
      <Component {...props} />
    </Suspense>
  );

  return Suspended;
};

export { withSuspend };
