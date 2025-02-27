export default function ComponentsStyles() {
  return (
    <>
      {/* ********************************** Types of elements ********************************** */}

      {/* ********************************** Structural elements */}

      <div style={{ backgroundColor: '#ff00ff', padding: '20px' }}>Here is a div tag - block element</div>
      <header>Here is a header tag - block element</header>
      <footer>Here is a footer tag - block element</footer>
      <article>Here is a article tag - block element</article>
      <section>Here is a section tag - block element</section>
      <span>Here is a span tag - inline element</span>

      {/* ********************************** Information and visual elements */}
      <p>Paragraph - block element</p>
      <h1>Heading 1 - block element</h1>
      <h2>Heading 2 - block element</h2>
      <h3>Heading 3 - block element</h3>
      <h4>Heading 4 - block element</h4>
      <h5>Heading 5 - block element</h5>
      <h6>Heading 6 - block element</h6>
      <p>
        <strong>Strong - inline element</strong>
        <b>Strong - inline element</b>
        <em>Italic - inline element</em>
        <i>Also Italic - inline element</i>
        <sup>Subscript- inline element</sup>
        <sub>Subscript - inline element</sub>
      </p>

      {/* ********************************** Grid demo */}

      <div className="row" style={{ marginBottom: '100px' }}>
        <div className="col-4 col-sm-6" style={{ backgroundColor: '#ff007f', height: '20px' }}></div>
        <div className="col-4 col-sm-6" style={{ backgroundColor: '#ffee00', height: '20px' }}></div>
        <div className="col-4 col-sm-6"style={{ backgroundColor: '#00f1ff', height: '20px' }}></div>   
      </div>

      {/* ********************************** Display grid demo */}

      <div className="grid-parent" style={{ marginBottom: '100px' }}>
        <div className="div1" style={{ backgroundColor: '#ff007f' }}>Cell 1</div>
        <div className="div2" style={{ backgroundColor: '#94ff00' }}>Cell 2</div>
        <div className="div3" style={{ backgroundColor: '#ffee00' }}>Cell 3</div>
        <div className="div4" style={{ backgroundColor: '#fa5e11' }}>Cell 4</div>
        <div className="div5" style={{ backgroundColor: '#00f1ff' }}>Cell 5</div>
      </div>


      {/* ********************************** Interactive elements */}
      <a href="#">Link</a>

    </>
  );

}
