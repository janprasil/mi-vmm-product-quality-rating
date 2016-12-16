import ImagePair from './ImagePair.react';
import React, { PropTypes as RPT, PureComponent as Component } from 'react';
import Slider from 'rc-slider';
import { connect } from 'react-redux';
import { fetchReferences, deleteAllReferences } from '../../common/api/actions';
import { FileUpload } from 'redux-file-upload';

@connect(state => ({
  references: state.api.getIn(['references', 'data'])
}), { fetchReferences, deleteAllReferences })
export default class References extends Component {

  static propTypes = {
    deleteAllReferences: RPT.func,
    fetchReferences: RPT.func.isRequired,
    references: RPT.array
  }

  componentDidMount() {
    const { fetchReferences } = this.props;
    fetchReferences();
  }

  renderReference(key, reference) {
    const { imageUrl, contourImageUrl, cannyTreshodLinking, cannyTreshold } = reference.toJS();
    console.log(reference.toJS())
    return (
      <ImagePair
        cannyTreshodLinking={cannyTreshodLinking}
        cannyTreshold={cannyTreshold}
        contourUrl={contourImageUrl}
        id={key}
        imageUrl={imageUrl}
        pairType="reference"
      />
    );
  }

  render() {
    const { references, deleteAllReferences } = this.props;

    return (
      <div>
        <div>
          <h1>Referenční obrázky</h1>
          {references && references.map((x, key) => this.renderReference(key, x))}
          <button onClick={() => deleteAllReferences()}>Smazat vše</button>
          <FileUpload
            allowedFileTypes={['jpg', 'jpeg', 'png']}
            data={{ type: 'picture' }}
            dropzoneId="fileUpload"
            multiple
            url="/webapi/reference"
          >
            <button>
              Klikněte nebo přetáhněte referenční obrázky (JPG)
            </button>
          </FileUpload>
        </div>
      </div>
    );
  }
}
