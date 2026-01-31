import styled from '@emotion/styled';
import { Button } from 'antd';
import { FC, SyntheticEvent, useCallback, useMemo, useState } from 'react';

const StyledButton = styled(Button)`
  outline: none !important;
  padding: 0;
`;

const ShowMoreTextTextField: FC<{ children: string | null | undefined; maxLenght?: number }> = ({
  children,
  maxLenght = 230,
}) => {
  const [readMore, setReadMore] = useState(false);

  const onReadMore = useCallback(
    (e: SyntheticEvent) => {
      setReadMore(!readMore);

      e.stopPropagation();
    },
    [readMore],
  );

  const showMoreButton = useMemo(() => {
    return children && children?.length > maxLenght;
  }, [children, maxLenght]);

  return (
    <div>
      <div className="text">
        {readMore || !showMoreButton ? children : `${children?.substring(0, maxLenght ?? 0)}...`}
      </div>
      {showMoreButton ? (
        <StyledButton type="link" onClick={onReadMore}>
          {readMore ? 'Скрыть' : 'Показать еще'}
        </StyledButton>
      ) : undefined}
    </div>
  );
};

export { ShowMoreTextTextField };
