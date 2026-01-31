import { FC, MouseEventHandler } from 'react';

import StyledContainer from './StyledContainer';
import StyledControl from './StyledControl';
import StyledItem from './StyledItem';

type DotsProps = {
  count?: number;
  onClick?: MouseEventHandler<HTMLButtonElement>;
  selected?: number;
  showSingle?: boolean;
};

const Dots: FC<DotsProps> = ({ count = 0, onClick, selected, showSingle }) => {
  const limit = showSingle ? 0 : 1;

  const renderDots = () => {
    const dots = [];

    for (let index = 0; index < count; index += 1) {
      dots.push(
        <StyledItem key={`${index}`}>
          <StyledControl active={index === selected} data-index={index} onClick={onClick} />
        </StyledItem>,
      );
    }

    return dots;
  };

  return count > limit ? <StyledContainer>{renderDots()}</StyledContainer> : null;
};

export { Dots };
export type { DotsProps };
