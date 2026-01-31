import styled from '@emotion/styled';
import { Button } from 'antd';

const StyledButton = styled(Button)<{ selected: boolean }>`
  font-size: 80%;
  height: auto;
  display: block;
  border-radius: 20px;
  ${(props) => {
    return props.selected
      ? `
      color: #7e004a;
      span {
        text-shadow: 0 0 1px;
      }
    `
      : ``;
  }}
`;

export { StyledButton };
